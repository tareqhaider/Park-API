﻿using Park_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Park_API.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int nationalParkId);

        bool NationalParkExists(string name);

        bool NationalParkExists(int id);

        bool CreateNationalPark(NationalPark nationalPark);

        bool UpdateNationalPark(NationalPark nationalPark);

        bool DeleteNationalPark(NationalPark nationalPark);

        bool SaveChanges();

    }
}
