using Kortyérzet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Models
{
    public class CheckinModel
    {
        public Checkin Checkin { get; set; }
        public Beer Beer { get; set; }
        public CheckinModel(Checkin checkin, Beer beer)
        {
            Checkin = checkin;
            Beer = beer;
        }
    }
}
