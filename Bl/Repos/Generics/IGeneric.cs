using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Repos.Generics
{
    public interface IGeneric<T> where T : class
    {
        public void Add(T entity);
        IEnumerable<T> GetAll();
        public  Task <T?> GetById(int id);
        public bool Delete(int id);
        public  bool Update(T entity);
    }
}

/*
1- author <=> Book
2- implement all api (category, author)
3- return Response<T> {
    bool Success;
    string[] Messages;
    T Data;
 }
4- add custom middleware to read acceppted-language from request header
 seperate Login from controller to service
 
 */