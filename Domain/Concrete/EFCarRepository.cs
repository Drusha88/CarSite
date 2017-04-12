using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFCarRepository : ICarRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Car> Cars
        {
            get { return context.Cars; }
        }

        public void SaveCar(Car car)
        {
            if (car.CarId == 0)
            {
                context.Cars.Add(car);
            }
            else
            {
                Car dbEntry = context.Cars.Find(car.CarId);
                if (dbEntry != null)
                {
                    dbEntry.Name = car.Name;
                    dbEntry.Brand = car.Brand;
                    dbEntry.Description = car.Description;
                    dbEntry.Category = car.Category;
                    dbEntry.ImageData = car.ImageData;
                    dbEntry.ImageMimeType = car.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Car DeleteCar(int carId)
        {
            Car dbEntry = context.Cars.Find(carId);
            if (dbEntry != null)
            {
                context.Cars.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
