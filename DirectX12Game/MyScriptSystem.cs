﻿using System;
using System.Linq;
using System.Numerics;
using DirectX12GameEngine;

namespace DirectX12Game
{
    public class MyScriptSystem : EntitySystem<MyScriptComponent>
    {
        private float time;
        private float scrollAmount = 50.0f;
        private readonly float scrollSpeed = 0.01f;

        public MyScriptSystem(IServiceProvider services) : base(services, typeof(TransformComponent))
        {
            if (Game.GameContext is GameContextCoreWindow context)
            {
                context.Control.KeyDown += Control_KeyDown;
                context.Control.PointerPressed += Control_PointerPressed;
                context.Control.PointerWheelChanged += Control_PointerWheelChanged;
            }
        }

        public override void Update(TimeSpan deltaTime)
        {
            //System.Diagnostics.Debug.WriteLine(1.0 / deltaTime.TotalSeconds);

            foreach (MyScriptComponent component in Components)
            {
                time += (float)deltaTime.TotalSeconds;

                Quaternion timeRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, time);

                var scene = SceneSystem.RootScene;

                if (scene != null)
                {
                    Entity camera = scene.FirstOrDefault(m => m.Name == "MyCamera");
                    if (camera != null)
                    {
                        SceneSystem.CurrentCamera = camera.Get<CameraComponent>();
                        camera.Transform.Position = new Vector3(0.0f, 300.0f, 10.0f * scrollAmount * 3);
                    }

                    Entity tRex = scene.FirstOrDefault(m => m.Name == "T-Rex");
                    if (tRex != null)
                    {
                        tRex.Transform.Rotation = timeRotation;
                    }

                    var rexes = scene.Where(m => m.Name == "T-Rex");
                    foreach (var item in rexes)
                    {
                        item.Transform.Rotation = timeRotation;
                    }

                    Entity video = scene.FirstOrDefault(m => m.Name == "MyVideo");
                    if (video != null)
                    {
                        VideoComponent videoComponent = video.Get<VideoComponent>();

                        if (videoComponent.Target is null)
                        {
                            Model? model = tRex?.Get<ModelComponent>().Model;
                            videoComponent.Target = model?.Materials[0].Textures[0];
                            //videoComponent.Target = GraphicsDevice.Presenter?.BackBuffer;
                        }
                    }

                    Entity cliffhouse = scene.FirstOrDefault(m => m.Name == "Cliffhouse");
                    if (cliffhouse != null)
                    {
                        cliffhouse.Transform.Position = new Vector3(1000.0f, 0.0f, 0.0f);
                        cliffhouse.Transform.Rotation = timeRotation;
                        cliffhouse.Transform.Scale = new Vector3(10.0f);
                    }

                    Entity rightHandModel = scene.FirstOrDefault(m => m.Name == "RightHandModel");
                    if (rightHandModel != null)
                    {
                        rightHandModel.Transform.Position = new Vector3(600.0f, 0.0f, 0.0f);
                        rightHandModel.Transform.Rotation = timeRotation;
                        rightHandModel.Transform.Scale = new Vector3(1000.0f);
                    }

                    Entity leftHandModel = scene.FirstOrDefault(m => m.Name == "HoloTile");
                    if (leftHandModel != null)
                    {
                        leftHandModel.Transform.Position = new Vector3(-300.0f, 0.0f, 0.0f);
                        leftHandModel.Transform.Rotation = timeRotation;
                        leftHandModel.Transform.Scale = new Vector3(10.0f);
                    }

                    Entity icon = scene.FirstOrDefault(m => m.Name == "Icon_Failure");
                    if (icon != null)
                    {
                        icon.Transform.Position = new Vector3(-800.0f, 0.0f, 0.0f);
                        icon.Transform.Rotation = timeRotation;
                        icon.Transform.Scale = new Vector3(10.0f);
                    }

                    Entity cube = scene.FirstOrDefault(m => m.Name == "LiveCube");
                    if (cube != null)
                    {
                        cube.Transform.Position = new Vector3(-1100.0f, 0.0f, 0.0f);
                        cube.Transform.Rotation = timeRotation;
                        cube.Transform.Scale = new Vector3(1.0f);
                    }
                }
            }
        }

        private void Control_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    scrollAmount--;
                    break;
                case Windows.System.VirtualKey.Down:
                    scrollAmount++;
                    break;
                case Windows.System.VirtualKey.Number0 when GraphicsDevice.Presenter != null:
                    GraphicsDevice.Presenter.PresentationParameters.SyncInterval = 0;
                    break;
                case Windows.System.VirtualKey.Number1 when GraphicsDevice.Presenter != null:
                    GraphicsDevice.Presenter.PresentationParameters.SyncInterval = 1;
                    break;
            }
        }

        private void Control_PointerPressed(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.IsLeftButtonPressed)
            {
                scrollAmount--;
            }
            else if (args.CurrentPoint.Properties.IsRightButtonPressed)
            {
                scrollAmount++;
            }
        }

        private void Control_PointerWheelChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        {
            scrollAmount -= args.CurrentPoint.Properties.MouseWheelDelta * scrollSpeed;
        }
    }
}