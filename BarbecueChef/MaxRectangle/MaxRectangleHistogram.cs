using System.Collections.Generic;

namespace BarbecueChef.MaxRectangle
{
    //https://www.geeksforgeeks.org/maximum-size-rectangle-binary-sub-matrix-1s/
    //Minor modifications, added Rectangle information (original algorithm used just area)
    public class MaxRectangleHistogram : IMaxRectangle
    {
        public Rectangle GetLargestRectangle(bool[,] surface)
        {
            int[][] area = new int[surface.GetLength(0)][];
            for (int i = 0; i < surface.GetLength(0); i++)
            {
                area[i] = new int[surface.GetLength(1)];
                for (int j = 0; j < surface.GetLength(1); j++)
                {
                    area[i][j] = surface[i, j] ? 0 : 1;
                }
            }
            return MaxRectangle(surface.GetLength(0), surface.GetLength(1), area);
        }

        private Rectangle MaxRectangleUnderHistogram(int width,
                              int[] row, int lengthPosition)
        {
            int rectangleLength = 0;
            int rectangleWidth = 0;
            int widthPosition = 0;

            // Create an empty stack. The stack 
            // holds indexes of hist[] array. 
            // The bars stored in stack are always 
            // in increasing order of their heights. 
            Stack<int> result = new Stack<int>();

            int top_val; // Top of stack 

            int max_area = 0; // Initialize max area in 
                              // current row (or histogram) 

            int area = 0; // Initialize area with 
                          // current top 

            // Run through all bars of 
            // given histogram (or row) 
            int i = 0;
            while (i < width)
            {
                // If this bar is higher than the 
                // bar on top stack, push it to stack 
                if (result.Count == 0 || row[result.Peek()] <= row[i])
                {
                    result.Push(i++);
                }

                else
                {
                    // If this bar is lower than top 
                    // of stack, then calculate area of 
                    // rectangle with stack top as 
                    // the smallest (or minimum height) 
                    // bar. 'i' is 'right index' for 
                    // the top and element before 
                    // top in stack is 'left index' 
                    top_val = row[result.Peek()];
                    result.Pop();
                    area = top_val * i;

                    if (result.Count > 0)
                    {
                        area = top_val * (i - result.Peek() - 1);
                    }
                    if (area > max_area)
                    {
                        if (result.Count > 0)
                        {
                            rectangleWidth = (i - result.Peek() - 1);
                        }
                        else
                        {
                            rectangleWidth = i;
                        }
                        rectangleLength = top_val;
                        widthPosition = i - rectangleWidth;
                        max_area = area;
                    }
                }
            }

            var widthDiff = i - result.Count;
            // Now pop the remaining bars from 
            // stack and calculate area with 
            // every popped bar as the smallest bar 
            while (result.Count > 0)
            {
                top_val = row[result.Peek()];
                var tempWidth = result.Peek();
                result.Pop();
                area = top_val * i;
                if (result.Count > 0)
                {
                    area = top_val * (i - result.Peek() - 1);
                }

                if (area > max_area)
                {
                    if (result.Count > 0)
                    {
                        rectangleWidth = (i - result.Peek() - 1);
                        widthPosition = tempWidth - (tempWidth - result.Peek() - 1);

                    }
                    else
                    {
                        rectangleWidth = i;
                        widthPosition = 0;
                    }
                    rectangleLength = top_val;
                    max_area = area;
                }
            }

            //if no rectangle found
            if (rectangleLength == 0 || rectangleWidth == 0)
            {
                return new Rectangle();
            }

            return new Rectangle()
            {
                Length = rectangleLength,
                Width = rectangleWidth,
                LengthPosition = lengthPosition - rectangleLength + 1,
                WidthPosition = widthPosition,
            };
        }

        // Returns area of the largest 
        // rectangle with all 1s in A[][] 
        private Rectangle MaxRectangle(int length, int width,
                                       int[][] A)
        {
            // Calculate rectangle for first row 
            // and initialize it as result 
            Rectangle maxRectangle = MaxRectangleUnderHistogram(width, A[0], 0);

            // iterate over row to find 
            // maximum rectangular area 
            // considering each row as histogram 
            for (int i = 1; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {

                    // if A[i][j] is 1 then 
                    // add A[i -1][j] 
                    if (A[i][j] == 1)
                    {
                        A[i][j] += A[i - 1][j];
                    }
                }

                // Update maxRectangle if area with current 
                // row (as last row of rectangle) is more 
                var rowRectangle = MaxRectangleUnderHistogram(width, A[i], i);
                if (rowRectangle.Length * rowRectangle.Width > maxRectangle.Length * maxRectangle.Width)
                {
                    maxRectangle = rowRectangle;
                }
            }

            return maxRectangle;
        }
    }
}
