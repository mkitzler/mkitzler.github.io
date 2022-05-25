export async function Init(canvasId, text) {
    // Get the canvas DOM element
    var canvas = document.getElementById(canvasId);
    // Load the 3D engine
    var engine = new BABYLON.Engine(canvas, true, { preserveDrawingBuffer: true, stencil: true });
    // CreateScene function that creates and return the scene
    var createScene = function () {
        // Create a basic BJS Scene object
        var scene = new BABYLON.Scene(engine);
        scene.clearColor = new BABYLON.Color4(0, 0, 0, 0);

        var camera = new BABYLON.ArcRotateCamera('camera1', -Math.PI / 2, Math.PI / 2, 10, new BABYLON.Vector3.Zero(), scene);
        // Attach the camera to the canvas
        //camera.attachControl(canvas, false);

        // Create a basic light, aiming 0, 1, 0 - meaning, to the sky
        var light = new BABYLON.HemisphericLight('light1', new BABYLON.Vector3(0, 1, 0), scene);

        //var sphere = BABYLON.MeshBuilder.CreateSphere('sphere1', { segments: 16, diameter: .5, sideOrientation: BABYLON.Mesh.FRONTSIDE }, scene);

        // Info: https://github.com/BabylonJS/Extensions/tree/master/MeshWriter

        var Writer = MeshWriter(scene, { scale: 0.1, defaultFont: "Arial", methods: BABYLON });
        var text1 = new Writer(
            text,
            {
                "anchor": "center",
                "letter-height": 10,
                "letter-thickness": 3,
                "color": "#1C3870",
                "position": {
                    y: -3.5,
                    z: -1.5
                }
            }
        );
        var textMesh = text1.getMesh()
        //textMesh.rotate(new BABYLON.Vector3(1, 0, 0), -Math.PI / 2)
        textMesh.rotation.x = -Math.PI / 2

        var initialPos = textMesh.position.x;

        var frameRate = 1;

        const yRotate = new BABYLON.Animation("yRotate", "rotation.y", frameRate, BABYLON.Animation.ANIMATIONTYPE_FLOAT, BABYLON.Animation.ANIMATIONLOOPMODE_CYCLE);
        var keyFrames = [];
        keyFrames.push({
            frame: 0,
            value: 0
        });
        keyFrames.push({
            frame: 6,
            value: -Math.PI
        });
        keyFrames.push({
            frame: 12,
            value: -Math.PI * 2
        });
        yRotate.setKeys(keyFrames);
        textMesh.animations.push(yRotate);

        const xTranslate = new BABYLON.Animation("xTranslate", "position.x", frameRate, BABYLON.Animation.ANIMATIONTYPE_FLOAT, BABYLON.Animation.ANIMATIONLOOPMODE_CYCLE);
        keyFrames = [];
        keyFrames.push({
            frame: 0,
            value: initialPos
        });
        keyFrames.push({
            frame: 6,
            value: -initialPos
        });
        keyFrames.push({
            frame: 12,
            value: initialPos
        });
        xTranslate.setKeys(keyFrames);
        textMesh.animations.push(xTranslate);

        const zTranslate = new BABYLON.Animation("zTranslate", "position.z", frameRate, BABYLON.Animation.ANIMATIONTYPE_FLOAT, BABYLON.Animation.ANIMATIONLOOPMODE_CYCLE);
        keyFrames = [];
        keyFrames.push({
            frame: 0,
            value: 0
        });
        keyFrames.push({
            frame: 3,
            value: initialPos
        });
        keyFrames.push({
            frame: 6,
            value: 0
        });
        keyFrames.push({
            frame: 9,
            value: -initialPos
        });
        keyFrames.push({
            frame: 12,
            value: 0
        });
        zTranslate.setKeys(keyFrames);
        textMesh.animations.push(zTranslate);

        scene.beginAnimation(textMesh, 0, 12, true);

        return scene;
    }
    // call the createScene function
    var scene = createScene();
    // run the render loop
    engine.runRenderLoop(function () {
        scene.render();
    });
    // the canvas/window resize event handler
    window.addEventListener('resize', function () {
        engine.resize();
    });
}