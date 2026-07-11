"""Inspeciona o blend oficial."""
import bpy

print("=== BLEND INSPECTION ===")
print(f"Objects: {len(bpy.data.objects)}")
print(f"Meshes: {len(bpy.data.meshes)}")
print("COLLECTIONS:")
for col in bpy.data.collections:
    meshes = [o.name for o in col.all_objects if o.type == "MESH"]
    print(f"  {col.name}: {len(meshes)} meshes")
    for m in meshes[:5]:
        obj = bpy.data.objects.get(m)
        mods = [mod.type for mod in obj.modifiers] if obj else []
        print(f"    - {m} mods={mods}")
    if len(meshes) > 5:
        print(f"    ... +{len(meshes)-5} more")

print("ALL_MESH_OBJECTS:")
for obj in bpy.data.objects:
    if obj.type == "MESH":
        mods = [f"{mod.name}:{mod.type}" for mod in obj.modifiers]
        print(f"  {obj.name} verts={len(obj.data.vertices)} mods=[{', '.join(mods)}]")
