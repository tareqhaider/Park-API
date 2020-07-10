using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Park_API.Data;
using Park_API.Models;

namespace Park_API.Repository.IRepository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TrailRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public bool CreateTrail(Trail Trail)
        {
            _dbContext.Add(Trail);

            return SaveChanges();
        }

        public bool UpdateTrail(Trail Trail)
        {
            _dbContext.Trails.Update(Trail);

            return SaveChanges();
        }


        public bool DeleteTrail(Trail Trail)
        {
            _dbContext.Remove(Trail);

            return SaveChanges();
        }

        public Trail GetTrail(int TrailId)
        {
            return _dbContext.Trails.Include(x => x.NationalPark)
                                    .FirstOrDefault(x => x.Id == TrailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _dbContext.Trails.Include(x => x.NationalPark)
                                    .OrderBy(x => x.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalpark(int nationalParkId)
        {
            return _dbContext.Trails.Include(x => x.NationalPark)
                                    .Where(x => x.NationalParkId == nationalParkId)
                                    .ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _dbContext.Trails.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());

            return value;
        }

        public bool TrailExists(int id)
        {
            bool value = _dbContext.Trails.Any(x => x.Id == id);

            return value;
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
            
        }

        
    }
}
