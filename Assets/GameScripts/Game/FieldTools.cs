using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{
    public static class FieldTools
    {
        public static List<Region> FillPointsWithShapes(HashSet<Vector2Int> points, List<ShapeData> shapeData)
        {
            var output = new List<Region>();
            var validRegions = FindAllValidRegions(points);
            foreach (var validRegion in validRegions)
            {
                var regionGraph = CreateGraphFromRegion(validRegion);
                var newRegions = FillRegionWithShapes(regionGraph, shapeData);
                output.AddRange(newRegions);
            }
            return output;
        }

        private static List<HashSet<Vector2Int>> FindAllValidRegions(HashSet<Vector2Int> points)
        {
            var validRegions = new List<HashSet<Vector2Int>>();

            while (points.Count > 0)
            {
                var startingPoint = points.First();
                HashSet<Vector2Int> validRegion = new HashSet<Vector2Int>(); 
                FindAllNeighbours(startingPoint, ref validRegion);
                validRegions.Add(validRegion);
            }
            return validRegions;

            void FindAllNeighbours(Vector2Int point, ref HashSet<Vector2Int> output)
            {
                points.Remove(point);
                var allN = point.Neighbours().Where(x => points.Contains(x));
                foreach (var neighbour in allN)
                {
                    points.Remove(neighbour);
                    FindAllNeighbours(neighbour, ref output);
                }
                output.Add(point);
            }
        }

        private static List<Region> CreateGraphFromRegion(HashSet<Vector2Int> region)
        {
            var regionsDictionary = new Dictionary<Vector2Int, Region>();
            foreach (var cell in region)
                regionsDictionary.Add(cell, new Region(cell));

            foreach (var cell in region)
            {
                foreach (var neighbour in cell.Neighbours().Where(x => region.Contains(x)))
                {
                    if(NeighbourIsRedundant(cell, neighbour))
                        continue;
                    regionsDictionary[cell].AddNeighbour(regionsDictionary[neighbour]);
                }
            }
            
            bool NeighbourIsRedundant(Vector2Int cell, Vector2Int neighbour)
            {
                return neighbour.IsDiagonal(cell) && 
                       (region.Contains(new Vector2Int(neighbour.x, cell.y)) || region.Contains(new Vector2Int(cell.x, neighbour.y)));
            }

            var output = regionsDictionary.Values.ToList();
            output.Reverse();
            return output;
        }
        
        private static List<Region> FillRegionWithShapes(List<Region> regions, List<ShapeData> shapeData)
        {
            var sortedShapes = shapeData.OrderByDescending(shape => shape.Rect.Perimeter())
                .ThenByDescending(shape => shape.Rect.x).ToList();

            if (regions.Count == 0)
                throw new InvalidOperationException("Cannot process empty regions list");
            var regionsSet = new List<Region>(regions);
            
            if (regionsSet.Count == 1)
            {
                var region = regionsSet.First();
                FindSuitableShape(region.cells, sortedShapes, region.rect, out var placementData);
                region.shapeId = placementData.shapeId;
                region.origin = placementData.origin;
                region.shapeRotation = placementData.rotation;
                return CreateRegionsListFromGraph(region);
            }

            while (regionsSet.Count > 1)
            {
                var minRegion = regionsSet.Min();
                regionsSet.Remove(minRegion);
                var minNeighbour = minRegion.neighbours.Min(); // TODO: Проверять всех соседей, а не только минимального
                if (Join(minRegion, minNeighbour, out var createdRegion))
                {
                    regionsSet.Add(createdRegion);
                    regionsSet.Remove(minNeighbour);
                }
                else
                {
                    FindSuitableShape(minRegion.cells, sortedShapes, minRegion.rect, out var placementData);
                    minRegion.shapeId = placementData.shapeId;
                    minRegion.origin = placementData.origin;
                    minRegion.shapeRotation = placementData.rotation;
                }
            }

            return CreateRegionsListFromGraph(regionsSet.First());

            bool Join(Region region, Region neighbour, out Region createdRegion)
            {
                createdRegion = null;
                var unitedCells = new List<Vector2Int>();
                unitedCells.AddRange(region.cells);
                unitedCells.AddRange(neighbour.cells);
                var unitedRect = FindRectFromCells(unitedCells);
                
                if (FindSuitableShape(unitedCells, sortedShapes, unitedRect, out var placementData))
                {
                    createdRegion = new Region(unitedCells, unitedRect, placementData.shapeId, placementData.origin, 
                        placementData.rotation);
                    var unitedNeighbours = new HashSet<Region>(region.neighbours);
                    unitedNeighbours.UnionWith(neighbour.neighbours);
                    unitedNeighbours.Remove(region);
                    unitedNeighbours.Remove(neighbour);
                    createdRegion.neighbours = unitedNeighbours;
                    foreach (var n in unitedNeighbours)
                    {
                        n.neighbours.Remove(region);
                        n.neighbours.Remove(neighbour);
                        n.AddNeighbour(createdRegion);
                    }
                    return true;
                }
                return false;
            }
        }

        private static List<Region> CreateRegionsListFromGraph(Region region)
        {
            var visited = new HashSet<Region>();
            var q = new Queue<Region>();
            q.Enqueue(region);
            visited.Add(region);

            while (q.Count != 0)
            {
                var vertex = q.Dequeue();
                foreach (var neighbour in vertex.neighbours)
                {
                    if(visited.Contains(neighbour))
                        continue;
                    visited.Add(neighbour);
                    q.Enqueue(neighbour);
                }
            }
            return visited.ToList();
        }

        private static bool FindSuitableShape(List<Vector2Int> cells, List<ShapeData> sortedShapes, Vector2Int rect, out PlacementData placementData)
        {
            placementData = null;
            foreach (var shape in sortedShapes.Where(shape => shape.Rect.SameRect(rect)))
            {
                foreach (var cell in cells)
                {
                    if (ShapeCanBePlaced(cell, shape, out placementData))
                    {
                        return true;
                    }
                }
            }
            return false;
            
            bool ShapeCanBePlaced(Vector2Int cell, ShapeData shape, out PlacementData placementData2)
            {
                placementData2 = null;
                foreach (Rotation rotation in Enum.GetValues(typeof(Rotation)))
                {
                    if (ShapeWithRotationCanBePlaced(shape, rotation, cell))
                    {
                        placementData2 = new PlacementData(shape.Uid, cell, rotation);
                        return true;
                    }
                }
                return false;
            }
            
            bool ShapeWithRotationCanBePlaced(ShapeData shape, Rotation rotation, Vector2Int cell)
            {
                var shapePointsOnField = new HashSet<Vector2Int>();
                foreach (var point in shape.PointsAfterRotation(rotation))
                {
                    var pointFieldPosition = cell + point;
                    shapePointsOnField.Add(pointFieldPosition);
                }
                return shapePointsOnField.SetEquals(cells);
            }
        }

        private static Vector2Int FindRectFromCells(List<Vector2Int> unitedCells)
        {
            int maxX = unitedCells.Max(vector => vector.x);
            int maxY = unitedCells.Max(vector => vector.y);
            int minX = unitedCells.Min(vector => vector.x);
            int minY = unitedCells.Min(vector => vector.y);

            return new Vector2Int(maxX - minX + 1, maxY - minY + 1);
        }

        private class PlacementData
        {
            public int shapeId;
            public Vector2Int origin;
            public Rotation rotation;

            public PlacementData(int shapeId, Vector2Int origin, Rotation rotation)
            {
                this.shapeId = shapeId;
                this.origin = origin;
                this.rotation = rotation;
            }
        }

        public class Region : IComparable<Region>
        {
            public Vector2Int rect;
            public List<Vector2Int> cells;
            public HashSet<Region> neighbours;
        
            public int shapeId;
            public Rotation shapeRotation;
            public Vector2Int origin;
        
            public readonly string uid;

            public Region(List<Vector2Int> cells)
            {
                this.cells = cells;
                neighbours = new HashSet<Region>();
                uid = Guid.NewGuid().ToString();
            }
        
            public Region(Vector2Int cell)
            {
                cells = new List<Vector2Int> {cell};
                rect = new Vector2Int(1, 1);
                neighbours = new HashSet<Region>();
                uid = Guid.NewGuid().ToString();
            }

            public Region(List<Vector2Int> cells, Vector2Int rect, int shapeId, Vector2Int origin, Rotation rotation)
            {
                this.cells = cells;
                this.rect = rect;
                this.shapeId = shapeId;
                this.origin = origin;
                shapeRotation = rotation;
                neighbours = new HashSet<Region>();
                uid = Guid.NewGuid().ToString();
            }

            public void AddNeighbour(Region neighbour)
            {
                neighbours.Add(neighbour);
            }

            public int CompareTo(Region other)
            {
                return cells.Count.CompareTo(other.cells.Count);
            }
        }
    }
}