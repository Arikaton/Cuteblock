using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.TrueShadow
{
interface IChangeTracker
{
    void Check();
}

class ChangeTracker<T> : IChangeTracker
{
    T                             previousValue;
    readonly Func<T>              getValue;
    readonly Func<T, T>           onChange;
    readonly IEqualityComparer<T> comparer;

    public ChangeTracker(Func<T>              getValue,
                         Func<T, T>           onChange,
                         IEqualityComparer<T> comparer = null)
    {
        this.getValue = getValue;
        this.onChange = onChange;
        this.comparer = comparer ?? EqualityComparer<T>.Default;

        previousValue = this.getValue();
    }

    public void Forget()
    {
        previousValue = getValue();
    }

    public void Check()
    {
        T newValue = getValue();
        if (!comparer.Equals(newValue, previousValue))
        {
            previousValue = onChange(newValue);
        }
    }
}

public partial class TrueShadow
{
    Action               checkHierarchyDirtiedDelegate;
    IChangeTracker[]     transformTrackers;
    ChangeTracker<int>[] hierachyTrackers;

    void InitInvalidator()
    {
        checkHierarchyDirtiedDelegate = CheckHierarchyDirtied;
        hierachyTrackers = new[] {
            new ChangeTracker<int>(
                () => transform.GetSiblingIndex(),
                newValue =>
                {
                    SetHierachyDirty();
                    return newValue; // + 1;
                }
            ),
            new ChangeTracker<int>(
                () =>
                {
                    if (shadowRenderer)
                        return shadowRenderer.transform.GetSiblingIndex();
                    return -1;
                },
                newValue =>
                {
                    SetHierachyDirty();
                    return newValue; // + 1;
                }
            )
        };

        transformTrackers = new IChangeTracker[] {
            new ChangeTracker<Vector3>(
                () => transform.position,
                newValue =>
                {
                    SetLayoutDirty();
                    return newValue;
                }
            ),
            new ChangeTracker<Quaternion>(
                () => transform.rotation,
                newValue =>
                {
                    SetLayoutDirty();
                    if (Cutout)
                        SetTextureDirty();
                    return newValue;
                }
            ),
        };

        Graphic.RegisterDirtyLayoutCallback(ForceLayoutTextureDirty);
        Graphic.RegisterDirtyVerticesCallback(ForceLayoutTextureDirty);
        Graphic.RegisterDirtyMaterialCallback(OnGraphicMaterialDirty);

        CheckHierarchyDirtied();
        CheckTransformDirtied();
    }

    void TerminateInvalidator()
    {
        if (Graphic)
        {
            Graphic.UnregisterDirtyLayoutCallback(ForceLayoutTextureDirty);
            Graphic.UnregisterDirtyVerticesCallback(ForceLayoutTextureDirty);
            Graphic.UnregisterDirtyMaterialCallback(OnGraphicMaterialDirty);
        }
    }

    void OnGraphicMaterialDirty()
    {
        ForceLayoutTextureDirty();
        shadowRenderer.UpdateMaterial();
    }

    internal void CheckTransformDirtied()
    {
        if (transformTrackers != null)
        {
            for (var i = 0; i < transformTrackers.Length; i++)
            {
                transformTrackers[i].Check();
            }
        }
    }

    internal void CheckHierarchyDirtied()
    {
        if (ShadowAsSibling && hierachyTrackers != null)
        {
            for (var i = 0; i < hierachyTrackers.Length; i++)
            {
                hierachyTrackers[i].Check();
            }
        }
    }

    internal void ForgetSiblingIndexChanges()
    {
        for (var i = 0; i < hierachyTrackers.Length; i++)
        {
            hierachyTrackers[i].Forget();
        }
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        ApplySerializedData();

        if (ProjectSettings.Instance.UseGlobalAngleByDefault)
        {
            UseGlobalAngle = true;
        }
    }
#endif

    protected override void OnTransformParentChanged()
    {
        base.OnTransformParentChanged();

        if (!isActiveAndEnabled) return;

        SetHierachyDirty();
        this.NextFrames(checkHierarchyDirtiedDelegate);
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        if (!isActiveAndEnabled) return;

        ForceLayoutTextureDirty();
    }


    protected override void OnDidApplyAnimationProperties()
    {
        if (!isActiveAndEnabled) return;

        ForceLayoutTextureDirty();
    }

    public void ModifyMesh(Mesh mesh)
    {
        if (!isActiveAndEnabled) return;

        if (SpriteMesh) Utility.SafeDestroy(SpriteMesh);
        SpriteMesh = Instantiate(mesh);

        ForceLayoutTextureDirty();
    }

    public void ModifyMesh(VertexHelper verts)
    {
        if (!isActiveAndEnabled) return;

        // For when pressing play while in prefab mode
        if (!SpriteMesh) SpriteMesh = new Mesh();
        verts.FillMesh(SpriteMesh);

        ForceLayoutTextureDirty();
    }

    void ForceLayoutTextureDirty()
    {
#if TMP_PRESENT
        if (Graphic is TMPro.TextMeshProUGUI tmp)
        {
            if (tmp.text.Length == 0)
                SpriteMesh = null;
            else
                SpriteMesh = tmp.mesh;
        }
#endif
        SetLayoutDirty();
        ForceTextureDirty();
    }
}
}
