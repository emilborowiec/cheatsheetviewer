using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QuickSheet.CheatSheetPanel
{
    /// <summary>
    /// Algorithm for arranging a set of boxes.
    /// Boxes must be aligned in vertical columns of uniform width.
    /// Aspect ratio of result arrangement must be as close as possible to given containerAspectRatio.
    /// Optimized for minimizing empty space (a secondary objective to achieving given aspect ratio).
    /// </summary>
    public static class CheatSheetLayout
    {
        public static List<Tuple<int, Size>> UIElementsToBoxes(List<UIElement> stackPanels)
        {
            return stackPanels.Select((p, index) => new Tuple<int, Size>(index, p.DesiredSize)).ToList();
        }

        public static Rect GetBoxRect(int boxId, List<List<Tuple<int, Size>>> arrangement)
        {
            var x = 0d;
            foreach (var column in arrangement)
            {
                var y = 0d;
                var columnWidth = GetColumnWidth(column);
                foreach (var tuple in column)
                {
                    if (tuple.Item1 == boxId)
                    {
                        return new Rect(x, y, columnWidth, tuple.Item2.Height);
                    }
                    y += tuple.Item2.Height;
                }

                x += GetColumnWidth(column);
            }

            throw new ArgumentOutOfRangeException(nameof(boxId), "Did not find box of given id");
        }

        public static List<List<Tuple<int, Size>>> ScaleArrangement(List<List<Tuple<int, Size>>> arrangement, double scale)
        {
            var scaledArrangement = new List<List<Tuple<int, Size>>>();
            
            foreach (var column in arrangement)
            {
                var scaledColumn = new List<Tuple<int, Size>>();
                scaledArrangement.Add(scaledColumn);
                foreach (var tuple in column)
                {
                    var scaledBox = new Tuple<int, Size>(
                        tuple.Item1, new Size(tuple.Item2.Width * scale, tuple.Item2.Height * scale));
                    scaledColumn.Add(scaledBox);
                }
            }

            return scaledArrangement;
        }

        public static List<Tuple<int, Size>> FindColumnWithBox(int boxId, List<List<Tuple<int, Size>>> arrangement)
        {
            foreach (var column in arrangement)
            {
                foreach (var tuple in column)
                {
                    if (tuple.Item1 == boxId) return column;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(boxId), "Did not find box of given id");
        }

        public static Size FindBoxInArrangement(int boxId, List<List<Tuple<int, Size>>> arrangement)
        {
            foreach (var column in arrangement)
            {
                foreach (var tuple in column)
                {
                    if (tuple.Item1 == boxId) return tuple.Item2;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(boxId), "Did not find box of given id");
        }

        public static double GetScaleToFit(Size availableSize, Size arrangementSize)
        {
            var widthScale = availableSize.Width / arrangementSize.Width;
            var heightScale = availableSize.Height / arrangementSize.Height;
            return Math.Min(widthScale, heightScale);
        }

        public static Size GetArrangementSize(List<List<Tuple<int, Size>>> arrangement)
        {
            if (arrangement == null) throw new ArgumentNullException(nameof(arrangement));
            if (arrangement.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(arrangement));
            }

            var width = arrangement.Sum(GetColumnWidth);
            var height = arrangement.Max(GetColumnHeight);
            return new Size(width, height);
        }
        
        public static List<List<Tuple<int, Size>>> ArrangeBoxesInColumnsToFitSpace(double containerAspectRatio, List<Tuple<int, Size>> boxes)
        {
            if (boxes == null) throw new ArgumentNullException(nameof(boxes));
            
            // store arrangements in dictionary with resulting ratio
            var arrangementScores = new Dictionary<List<List<Tuple<int, Size>>>, double>();
            
            // Try all combinations of amounts of columns
            for (var i = 1; i <= boxes.Count; i++)
            {
                var arrangement = SplitToColumns(boxes, i);
                arrangementScores[arrangement] = GetAspectClosenessScore(GetArrangementAspect(arrangement), containerAspectRatio);
            }
            
            // Sort by closeness to perfect ratio
            return arrangementScores.OrderBy(pair => pair.Value).First().Key;
        }

        public static List<List<Tuple<int, Size>>> SplitToColumns(List<Tuple<int, Size>> boxes, int columnCount)
        {
            if (boxes == null) throw new ArgumentNullException(nameof(boxes));
            if (boxes.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(boxes));
            if (columnCount > boxes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount),
                    "Number of columns must be not more than box count");
            }

            var columns = new List<List<Tuple<int, Size>>>();
            
            var sortedQueue = new Queue<Tuple<int, Size>>(boxes.OrderBy(s => s.Item2.Height));
            
            var startHeight = GetColumnHeight(boxes);
            var desiredHeight = boxes.Max(b => b.Item2.Height);//startHeight / columnCount;

            for (var i = 0; i < columnCount; i++)
            {
                var column = new List<Tuple<int, Size>>();
                columns.Add(column);
            }

            while (sortedQueue.Count > 0)
            {
                var col = columns.OrderBy(GetColumnHeight)
                                 .FirstOrDefault();
                var box = sortedQueue.Dequeue();
                col.Add(box);
            }

            return columns;
        }

        /// <summary>
        /// The lower the value the closer is aspect tot targetAspect
        /// </summary>
        public static double GetAspectClosenessScore(double aspect, double targetAspect)
        {
            return Math.Abs(1 - (targetAspect / aspect));
        }

        public static double GetArrangementAspect(List<List<Tuple<int, Size>>> columns)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));
            if (columns.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(columns));
            }

            var width = columns.Sum(GetColumnWidth);
            var height = columns.Max(GetColumnHeight);
            return width / height;
        }
        
        public static double GetColumnAspect(List<Tuple<int, Size>> column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));
            if (column.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(column));
            
            var width = GetColumnWidth(column);
            var height = GetColumnHeight(column);
            return width / height;
        }

        private static double GetColumnHeight(List<Tuple<int, Size>> column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));
            if (column.Count == 0) return 0;
            
            return column.Sum(b => b.Item2.Height);
        }

        private static double GetColumnWidth(List<Tuple<int, Size>> column)
        {
            if (column == null) throw new ArgumentNullException(nameof(column));
            if (column.Count == 0) return 0;
            
            return column.Max(b => b.Item2.Width);
        }

        public static double GetBoxAspect(Size box)
        {
            return box.Width / box.Height;
        }
        
    }
}