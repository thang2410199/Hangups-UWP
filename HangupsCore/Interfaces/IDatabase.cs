using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangupsCore.Interfaces
{
    public interface IDatabase
    {
        /// <summary>
        /// check and create database if needed
        /// </summary>
        /// <param name="name"></param>
        void CreateTable<T>();

        IEnumerable<T> GetItems<T>(bool ordered = true) where T : class, ISQLItem;

        void AddItem<T>(T item) where T : ISQLItem;

        void Delete<T>(T item) where T : ISQLItem;
        T GetItem<T>(int id) where T : class, ISQLItem;
    }
}
