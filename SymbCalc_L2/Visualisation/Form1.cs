using System.Collections.Generic;
using WpfApplication1;

namespace SymbCalc_L2
{
    using System.Windows.Forms;
    using OxyPlot;
    using OxyPlot.Series;

    public partial class Form1 : Form
    {
        public Form1(List<DataPoint>[] points)
        {
            InitializeComponent();
            
            PlotModel myModel = new PlotModel { Title = "Example 1" };
            // Add each point to the new series
            
            foreach (var pointArray in points)
            {
                LineSeries pointLine = new LineSeries
                    { StrokeThickness = 2, MarkerSize = 1};
                
                foreach (var point in pointArray)
                {
                    pointLine.Points.Add(point);
                }

                myModel.Series.Add(pointLine);
            }
            plot1.Model = myModel;

        }
    }
}