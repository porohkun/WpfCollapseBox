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

        public static readonly DependencyProperty CollapsedContentProperty =
            DependencyProperty.Register(nameof(CollapsedContent), typeof(object), typeof(CollapseBox), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ExpandedContentProperty =
            DependencyProperty.Register(nameof(ExpandedContent), typeof(object), typeof(CollapseBox), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ExpandTimeProperty =
            DependencyProperty.Register(nameof(ExpandTime), typeof(TimeSpan), typeof(CollapseBox), new PropertyMetadata(new TimeSpan(TimeSpan.TicksPerMillisecond * 200)));

        public static readonly DependencyProperty TickThicknessProperty =
            DependencyProperty.Register(nameof(TickThickness), typeof(double), typeof(CollapseBox), new PropertyMetadata(1d));

        public static readonly DependencyProperty CollapsedHeightProperty =
            DependencyProperty.Register(nameof(CollapsedHeight), typeof(double), typeof(CollapseBox), new PropertyMetadata(20d));

        public static readonly DependencyProperty ExpandedHeightProperty =
            DependencyProperty.Register(nameof(ExpandedHeight), typeof(double), typeof(CollapseBox), new PropertyMetadata(100d));

        RotateTransform _pathTransform;
        ContentControl _collapsedContent;
        ContentControl _expandedContent;

        public CollapseBox() : base()
        {

        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _pathTransform = GetTemplateChild("PART_PathTransform") as RotateTransform;
            _collapsedContent = GetTemplateChild("PART_CollapsedContent") as ContentControl;
            _expandedContent = GetTemplateChild("PART_ExpandedContent") as ContentControl;
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            
            var pathAnim = new DoubleAnimation(-90, new Duration(ExpandTime));
            var heightAnim = new DoubleAnimation(ExpandedHeight, new Duration(ExpandTime));
            heightAnim.Completed += (sender, ea) =>
            {
                _collapsedContent.Visibility = Visibility.Collapsed;
                _expandedContent.Visibility = Visibility.Visible;
            };
            _collapsedContent.Visibility = Visibility.Collapsed;
            _expandedContent.Visibility = Visibility.Collapsed;

            _pathTransform.BeginAnimation(RotateTransform.AngleProperty, pathAnim);
            BeginAnimation(HeightProperty, heightAnim);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            
            var pathAnim = new DoubleAnimation(90, new Duration(ExpandTime));
            var heightAnim = new DoubleAnimation(CollapsedHeight, new Duration(ExpandTime));
            heightAnim.Completed += (sender, ea) =>
            {
                _collapsedContent.Visibility = Visibility.Visible;
                _expandedContent.Visibility = Visibility.Collapsed;
            };
            _collapsedContent.Visibility = Visibility.Collapsed;
            _expandedContent.Visibility = Visibility.Collapsed;

            _pathTransform.BeginAnimation(RotateTransform.AngleProperty, pathAnim);
            BeginAnimation(HeightProperty, heightAnim);

        }
    }
}
