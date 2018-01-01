﻿using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    class CounterpartyService : ICounterpartyService
    {
        private ICounterpartyDAO DAO;
        private IMapperBase<Counterparty> mapper;

        public CounterpartyService(ICounterpartyDAO DAO, IMapperBase<Counterparty> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        public List<Counterparty> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }
        
        public Counterparty GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        public List<Entity.CounterpartysStock> GetCounterpartysStock(Counterparty counterparty,
            IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper)
        {
            return counterpartysStockMapper.MapAllFrom(
                DAO.GetCounterpartysStock(mapper.MapTo(counterparty)));
        }
        
    }
}
