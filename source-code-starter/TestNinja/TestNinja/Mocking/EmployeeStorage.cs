using System;

namespace TestNinja.Mocking
{
    public interface IEmployeeStorage
    {
        void RemoveEmployee(int id);
    }

    public class EmployeeStorage : IEmployeeStorage
    {
        private EmployeeContext _db;

        public EmployeeStorage()
        {
            _db = new EmployeeContext();
        }
        
        public void RemoveEmployee(int id)
        {
            try
            {
                var employee = _db.Employees.Find(id);
                if (employee == null) return;
                _db.Employees.Remove(employee);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}