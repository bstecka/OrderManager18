﻿using OrderManager.DAL.InternalSysDAO;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    class EligibleORersNamesService : IEligibleOrdersNamesService
    {
        private IEligibleOrdersNamesDAO eligibleOrdersNamesDAO;
        private IMapperBase<string> mapper;

        public EligibleORersNamesService(IEligibleOrdersNamesDAO eligibleORersNamesDAO, IMapperBase<string> mapper)
        {
            this.eligibleOrdersNamesDAO = eligibleORersNamesDAO;
            this.mapper = mapper;
        }

        public List<string> FetchNames(int numberOfNames)
        {
            var names = mapper.MapAllFrom(eligibleOrdersNamesDAO.FetchNames(numberOfNames));
            if (names.Count() < numberOfNames)
                throw new Exception("Brak wolnych nazwa zamówień");
            return names;
        }
    }
}
