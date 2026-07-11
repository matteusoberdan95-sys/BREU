import bpy, sys
glb = sys.argv[sys.argv.index("--") + 1]
bpy.ops.object.select_all(action="SELECT")
bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.gltf(filepath=glb)
for obj in sorted(bpy.data.objects, key=lambda o: o.name):
    n = obj.name.lower()
    if any(k in n for k in ("sign", "text", "label", "offer", "recep", "deposit", "102", "pensao")):
        print(obj.name)
