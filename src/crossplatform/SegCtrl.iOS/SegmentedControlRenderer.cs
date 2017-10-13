﻿using System;
using Plugin.SegmentedControl.iOS;
using Plugin.SegmentedControl.Netstandard.Control;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SegmentedControl), typeof(SegmentedControlRenderer))]
namespace Plugin.SegmentedControl.iOS
{
    public class SegmentedControlRenderer : ViewRenderer<Netstandard.Control.SegmentedControl, UISegmentedControl>
    {
        private UISegmentedControl _nativeControl;

        protected override void OnElementChanged(ElementChangedEventArgs<Netstandard.Control.SegmentedControl> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _nativeControl = new UISegmentedControl();

                for (var i = 0; i < Element.Children.Count; i++)
                {
                    _nativeControl.InsertSegment(Element.Children[i].Text, i, false);
                }

                _nativeControl.Enabled = Element.IsEnabled;
                _nativeControl.TintColor = Element.IsEnabled ? Element.TintColor.ToUIColor() : Color.Gray.ToUIColor();
                SetSelectedTextColor();

                _nativeControl.SelectedSegment = Element.SelectedSegment;

                SetNativeControl(_nativeControl);
            }

            if (e.OldElement != null)
            {
                if (_nativeControl != null) _nativeControl.ValueChanged -= NativeControl_ValueChanged;
            }

            if (e.NewElement != null)
            {
                if (_nativeControl != null) _nativeControl.ValueChanged += NativeControl_ValueChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Renderer":
                    Element?.RaiseSelectionChanged();
                    break;
                case "SelectedSegment":
                    if (_nativeControl != null && Element != null)
                    {
                        _nativeControl.SelectedSegment = Element.SelectedSegment;
                        Element.RaiseSelectionChanged();
                    }
                    break;
                case "TintColor":
                    if (_nativeControl != null && Element != null)
                    {
                        _nativeControl.TintColor = Element.IsEnabled ? Element.TintColor.ToUIColor() : Color.Gray.ToUIColor();
                    }
                    break;
                case "IsEnabled":
                    if (_nativeControl != null && Element != null)
                    {
                        _nativeControl.Enabled = Element.IsEnabled;
                        _nativeControl.TintColor = Element.IsEnabled ? Element.TintColor.ToUIColor() : Color.Gray.ToUIColor();
                    }
                    break;
                case "SelectedTextColor":
                    if (_nativeControl != null && Element != null)
                    {
                        SetSelectedTextColor();
                    }
                    break;
                default:
                    break;
            }

        }

        private void SetSelectedTextColor()
        {
            var attr = new UITextAttributes {TextColor = Element.SelectedTextColor.ToUIColor()};
            _nativeControl.SetTitleTextAttributes(attr, UIControlState.Selected);
        }

        private void NativeControl_ValueChanged(object sender, EventArgs e)
        {
            Element.SelectedSegment = (int)_nativeControl.SelectedSegment;
        }

        protected override void Dispose(bool disposing)
        {
            if (_nativeControl != null)
            {
                _nativeControl.ValueChanged -= NativeControl_ValueChanged;
                _nativeControl.Dispose();
                _nativeControl = null;
            }

            try
            {
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public new static void Init()
        {
        }
    }
}