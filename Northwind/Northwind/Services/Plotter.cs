using Northwind.Data;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.WindowsForms;
using Northwind.Services.Interfaces;

namespace Northwind.Services
{
    /// <summary>
    /// Class using Oxyplot package to generate windows forms graphs and charts.
    /// </summary>
    public class Plotter : IPlotter
    {
        /// <summary>
        /// Function to plot products per category
        /// </summary>
        /// <param name="_context">The NW database</param>
        public void PlotCategories(NorthwindContext context)
        {
            var categoryCounts = context.Categories
                .Select(c => new
                {
                    CategoryName = c.CategoryName,
                    ProductCount = c.Products.Count(p => p.CategoryId == c.CategoryId)
                })
                .ToList();

            var model = new PlotModel { Title = "Products per Category" };
            var barSeries = new BarSeries
            {
                Title = "Products"
            };
            foreach (var category in categoryCounts)
            {
                barSeries.Items.Add(new BarItem { Value = category.ProductCount });
            }
            model.Series.Add(barSeries);

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            foreach (var category in categoryCounts)
            {
                categoryAxis.Labels.Add(category.CategoryName);
            }
            model.Axes.Add(categoryAxis);

            var plotView = new PlotView { Dock = DockStyle.Fill, Model = model };

            var form = new Form { Width = 800, Height = 600 };
            form.Controls.Add(plotView);
            Application.Run(form);
        }
    }
}