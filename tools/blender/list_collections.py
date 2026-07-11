"""Lista collections World_* no blend."""
import bpy
for col in sorted(bpy.data.collections, key=lambda c: c.name):
    if col.name.startswith("World_") or col.name.startswith("Godot_"):
        meshes = sum(1 for o in col.all_objects if o.type == "MESH")
        bevel = sum(1 for o in col.all_objects if o.type == "MESH" and any(m.type == "BEVEL" for m in o.modifiers))
        print(f"{col.name}: meshes={meshes} with_bevel={bevel}")
