//-----------------------------------------------------------------------
// <copyright file="ViewObjectContent.cs" company="Lifeprojects.de">
//     Class: ViewObjectContent
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>07.01.2021</date>
//
// <summary>
// Klasse zu Definition eines ViewObjects (Control/Window)
// </summary>
//-----------------------------------------------------------------------

namespace EVOSnext.Core
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using EasyPrototyping.Core;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;

    using Solarertrag.Core;

    public sealed class ViewObjectContent : BindingProperty<ViewObjectContent>
    {
        public ViewObjectContent(MenuButtons menueObject, string text)
        {
            this.MenueObject = menueObject;
            this.Command = $"{menueObject.ToString()}Command";
            this.Text = text;
            this.Description = string.Empty;
            this.ViewControl = null;
            this.Opacity = 0;
        }

        public ViewObjectContent()
        {
            this.Command = string.Empty;
            this.Text = string.Empty;
        }

        public MenuButtons MenueObject { get; set; } = MenuButtons.None;

        public ViewObjectTyp ViewObject { get; set; } = ViewObjectTyp.None;

        public string Command { get; set; }

        public Type ViewControl { get; set; }

        [PropertyBinding]
        public string Text
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string Description
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public double Opacity
        {
            get { return this.Get<double>(); }
            set { this.Set(value); }
        }

        public void AddUserControl(MenuButtons key, string text, Type viewControl, double startOpacity = 1)
        {
            if (key.GetType().IsEnum == false)
            {
                throw new Exception($"Der Key '{key}' muß vom Typ Enum sein => {this}");
            }

            this.MenueObject = key;
            this.Command = $"{key.ToString()}Command";
            this.Text = text;
            this.ViewControl = viewControl;
            this.Opacity = startOpacity;
            this.ViewObject = ViewObjectTyp.UserControl;
        }

        public void AddUserControl(string key, string text, Type viewControl, double startOpacity = 1)
        {
            if (key.GetType().IsEnum() == false)
            {
                throw new Exception($"Der Key '{key}' muß vom Typ Enum sein => {this}");
            }

            MenuButtons mobject = key.ToEnum<MenuButtons>();
            this.Command = $"{key.ToString()}Command";
            this.Text = text;
            this.ViewControl = viewControl;
            this.Opacity = startOpacity;
            this.ViewObject = ViewObjectTyp.UserControl;
        }

        public void AddWindowDialog(MenuButtons key, string text, Type viewWindow, double startOpacity = 1)
        {
            if (key.GetType().IsEnum() == false)
            {
                throw new Exception($"Der Key '{key}' muß vom Typ Enum sein => {this}");
            }

            this.MenueObject = key;
            this.Command = $"{key.ToString()}Command";
            this.Text = text;
            this.ViewControl = viewWindow;
            this.ViewObject = ViewObjectTyp.WindowDialog;
        }

        public void AddWindowDialog(string key, string text, Type viewWindow, double startOpacity = 1)
        {
            if (key.GetType().IsEnum() == false)
            {
                throw new Exception($"Der Key '{key}' muß vom Typ Enum sein => {this}");
            }

            MenuButtons mobject = key.ToEnum<MenuButtons>();
            this.Command = $"{key.ToString()}Command";
            this.Text = text;
            this.ViewControl = viewWindow;
            this.ViewObject = ViewObjectTyp.WindowDialog;
        }
    }
}
