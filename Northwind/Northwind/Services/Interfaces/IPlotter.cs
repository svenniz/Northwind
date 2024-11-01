using Northwind.Data;

namespace Northwind.Services.Interfaces
{
    public interface IPlotter
    {
        void PlotCategories(NorthwindContext context);
    }
}