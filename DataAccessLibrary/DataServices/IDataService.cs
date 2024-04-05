﻿namespace DataAccessLibrary.DataServices;

public interface IDataService<T> where T : class
{
    Task<T> GetAsync(int id);
    IQueryable<T> GetAll(string sortOrder, string sortColumn);
    Task<bool> CreateAsync(T model);
}