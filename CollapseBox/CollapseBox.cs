using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfCollapseBox
{
    [TemplatePart(Name = "PART_PathTransform", Type = typeof(RotateTransform))]
    public class CollapseBox : ToggleButton
    {
        public object CollapsedContent
        {
            get { return GetValue(CollapsedContentProperty); }
            set { SetValue(CollapsedContentProperty, value); }
        }

        public object ExpandedContent
        {
            get { return GetValue(ExpandedContentProperty); }
            set { SetValue(ExpandedContentProperty, value); }
        }

        public TimeSpan ExpandTime
        {
            get { return (TimeSpan)GetValue(ExpandTimeProperty); }
            set { SetValue(ExpandTimeProperty, value); }
        }

        public double TickThickness
        {
            get { return (double)GetValue(TickThicknessProperty); }
            set { SetValue(TickThicknessProperty, value); }
        }

        public double TickSize
        {
            get { return (double)GetValue(TickSizeProperty); }
            set { SetValue(TickSizeProperty, value); }
        }

        public bool RevertTick
        {
            get { return (bool)GetValue(RevertTickProperty); }
            set { SetValue(RevertTickProperty, value); }
        }

        public VerticalAlignment TickVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(TickVerticalAlignmentProperty); }
            set { SetValue(TickVerticalAlignmentProperty, value); }
        }

        public double CollapsedHeight
        {
            get { return (double)GetValue(CollapsedHeightProperty); }
            set
            {
                SetValue(CollapsedHeightProperty, value);
                if (!IsChecked.Value)
                    Height = CollapsedHeight;
            }
        }

        public double ExpandedHeight
        {
            get { return (double)GetValue(ExpandedHeightProperty); }
            set
            {
                SetValue(ExpandedHeightProperty, value);
                if (IsChecked.Value)
                    Height = ExpandedHeight;
            }
        }

        public bool ExpandOverContent
        {
            get { return (bool)GetValue(ExpandOverContentProperty); }
            set { SetValue(ExpandOverContentProperty, value); }
        }

        public static readonly DependencyProperty CollapsedContentProperty =
            DependencyProperty.Register(nameof(CollapsedContent), typeof(object), typeof(CollapseBox), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ExpandedContentProperty =
            DependencyProperty.Register(nameof(ExpandedContent), typeof(object), typeof(CollapseBox), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ExpandTimeProperty =
            DependencyProperty.Register(nameof(ExpandTime), typeof(TimeSpan), typeof(CollapseBox), new PropertyMetadata(new TimeSpan(TimeSpan.TicksPerMillisecond * 200)));

        public static readonly DependencyProperty TickThicknessProperty =
            DependencyProperty.Register(nameof(TickThickness), typeof(double), typeof(CollapseBox), new PropertyMetadata(1d));

        public static readonly DependencyProperty CollapsedHeightProperty =
            DependencyProperty.Register(nameof(CollapsedHeight), typeof(double), typeof(CollapseBox), new PropertyMetadata(20d, PropertyChanged));

        public static readonly DependencyProperty TickSizeProperty =
            DependencyProperty.Register(nameof(TickSize), typeof(double), typeof(CollapseBox), new PropertyMetadata(12d, PropertyChanged));

        public static readonly DependencyProperty RevertTickProperty =
            DependencyProperty.Register(nameof(RevertTick), typeof(bool), typeof(CollapseBox), new PropertyMetadata(false, PropertyChanged));

        public static readonly DependencyProperty TickVerticalAlignmentProperty =
            DependencyProperty.Register(nameof(TickVerticalAlignment), typeof(VerticalAlignment), typeof(CollapseBox), new PropertyMetadata(VerticalAlignment.Bottom));

        public static readonly DependencyProperty ExpandedHeightProperty =
            DependencyProperty.Register(nameof(ExpandedHeight), typeof(double), typeof(CollapseBox), new PropertyMetadata(100d, PropertyChanged));

        public static readonly DependencyProperty ExpandOverContentProperty =
            DependencyProperty.Register(nameof(ExpandOverContent), typeof(bool), typeof(CollapseBox), new PropertyMetadata(false));

        RotateTransform _pathTransform;
        ContentControl _collapsedContent;
        ContentControl _expandedContent;

        public CollapseBox() : base()
        {
            SetContentsVisibility(IsChecked.HasValue ? IsChecked.Value : false);
        }

        private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CollapseBox)
            {
                if (e.Property == ExpandedHeightProperty)
                    (d as CollapseBox).ExpandedHeightChanged(e);
            }
        }

        private void ExpandedHeightChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsChecked.Value)
                Height = ExpandedHeight;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _pathTransform = GetTemplateChild("PART_PathTransform") as RotateTransform;
            _collapsedContent = GetTemplateChild("PART_CollapsedContent") as ContentControl;
            _expandedContent = GetTemplateChild("PART_ExpandedContent") as ContentControl;
            SetContentsVisibility(IsChecked.HasValue ? IsChecked.Value : false);
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);

            var pathAnim = new DoubleAnimation(-90 * (RevertTick ? -1 : 1), new Duration(ExpandTime));
            var heightAnim = new DoubleAnimation(ExpandedHeight, new Duration(ExpandTime));
            heightAnim.Completed += (sender, ea) =>
            {
                SetContentsVisibility(true);
            };
            if (_collapsedContent != null)
                _collapsedContent.Visibility = Visibility.Collapsed;
            if (_expandedContent != null)
                _expandedContent.Visibility = Visibility.Collapsed;

            if (_pathTransform != null)
                _pathTransform.BeginAnimation(RotateTransform.AngleProperty, pathAnim);
            if (Template != null)
                BeginAnimation(HeightProperty, heightAnim);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);

            var pathAnim = new DoubleAnimation(90 * (RevertTick ? -1 : 1), new Duration(ExpandTime));
            var heightAnim = new DoubleAnimation(CollapsedHeight, new Duration(ExpandTime));
            heightAnim.Completed += (sender, ea) =>
            {
                SetContentsVisibility(false);
            };
            if (_collapsedContent != null)
                _collapsedContent.Visibility = Visibility.Collapsed;
            if (_expandedContent != null)
                _expandedContent.Visibility = Visibility.Collapsed;

            if (_pathTransform != null)
                _pathTransform.BeginAnimation(RotateTransform.AngleProperty, pathAnim);
            if (Template != null)
                BeginAnimation(HeightProperty, heightAnim);
        }

        protected void SetContentsVisibility(bool expand)
        {
            Height = expand ? ExpandedHeight : CollapsedHeight;
            if (_pathTransform != null)
                _pathTransform.Angle = (expand ? -90 : 90) * (RevertTick ? -1 : 1);
            if (_collapsedContent != null)
                _collapsedContent.Visibility = expand ? Visibility.Collapsed : Visibility.Visible;
            if (_expandedContent != null)
                _expandedContent.Visibility = expand ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
