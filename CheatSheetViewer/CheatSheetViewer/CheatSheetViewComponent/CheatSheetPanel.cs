using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CheatSheetViewerApp.CheatSheetViewComponent
{
    public class CheatSheetPanel : Panel
    {
        private List<List<Tuple<int, Size>>> _arrangement;
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_arrangement == null) return finalSize;
            
            var arrangementSize = CheatSheetLayout.GetArrangementSize(_arrangement);
            var scale = CheatSheetLayout.GetScaleToFit(finalSize, arrangementSize);
            _arrangement = CheatSheetLayout.ScaleArrangement(_arrangement, scale);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Arrange(CheatSheetLayout.GetBoxRect(i, _arrangement));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children.Count == 0)
            {
                _arrangement = null;
                return availableSize;
            }
            
            var aspectRatio = CheatSheetLayout.GetBoxAspect(availableSize);

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
            }

            var childrenList = new List<UIElement>();
            foreach (UIElement child in Children)
            {
                childrenList.Add(child);
            }

            var boxes = CheatSheetLayout.UIElementsToBoxes(childrenList);

            _arrangement = CheatSheetLayout.ArrangeBoxesInColumnsToFitSpace(aspectRatio, boxes);

            return availableSize;
        }
    }
}