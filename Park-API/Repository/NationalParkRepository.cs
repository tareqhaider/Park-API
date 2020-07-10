using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Park_API.Data;
using Park_API.Models;

namespace Park_API.Repository.IRepository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NationalParkRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _dbContext.Add(nationalPark);

            return SaveChanges();
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _dbContext.NationalParks.Update(nationalPark);

            return SaveChanges();
        }


        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _dbContext.Remove(nationalPark);

            return SaveChanges();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _dbContext.NationalParks.FirstOrDefault(x => x.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _dbContext.NationalParks.OrderBy(x => x.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _dbContext.NationalParks.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());

            return value;
        }

        public bool NationalParkExists(int id)
        {
            bool value = _dbContext.NationalParks.Any(x => x.Id == id);

            return value;
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        
    }
}
