using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();

            return allStates;
        }

        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;

            // submit changes
            db.SubmitChanges();
        }

        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        //// TODO Items: ////

        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            Employee employeeFromDb;
            switch (crudOperation)
            {

                case "create":
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    break;
                case "read":                    
                    employeeFromDb = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();//This probably doesnt return anything
                    break;
                case "update":
                    employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();
                    employeeFromDb.FirstName = employee.FirstName;
                    employeeFromDb.LastName = employee.LastName;
                    employeeFromDb.EmployeeNumber = employee.EmployeeNumber;
                    employeeFromDb.Email = employee.Email;
                    db.SubmitChanges();
                    break;
                case "delete":
                    db.Employees.DeleteOnSubmit(employee);
                    db.SubmitChanges();
                    break;
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)
        {
            Animal foundAnimal = db.Animals.Where(a => a.AnimalId == id).FirstOrDefault();
            return foundAnimal;
        }

        internal static void UpdateAnimal(Animal animal, Dictionary<int, string> updates)
        {
            if (updates.ContainsKey(1))
            {
                animal.Category.Name = (updates[1]);
            }
            if (updates.ContainsKey(2))
            {
                animal.Name = updates[2];
            }
            if (updates.ContainsKey(3))
            {
                animal.Age = Int32.Parse(updates[3]);
            }
            if (updates.ContainsKey(4))
            {
                animal.Demeanor = updates[4];
            }
            if (updates.ContainsKey(5))
            {
                animal.KidFriendly = Boolean.Parse(updates[5]);
            }
            if (updates.ContainsKey(6))
            {
                animal.PetFriendly = Boolean.Parse(updates[6]);
            }
            if (updates.ContainsKey(7))
            {
                animal.Weight = Int32.Parse(updates[7]);
            }

            db.SubmitChanges();

        }

        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }

        // TODO: Animal Multi-Trait Search
        internal static List<Animal> SearchForAnimalByMultipleTraits(Dictionary<int, string> updates) { // parameter(s)???????

            List<Animal> animals = new List<Animal>();
            if (updates.ContainsKey(1))
            {
                var primaryKey = db.Categories.Where(c => c.Name == updates[1]).Select(p => p.CategoryId).Single();
                var animalType = db.Animals.Where(a => a.CategoryId == primaryKey);
                animals.AddRange(animalType);
            }
            if (updates.ContainsKey(2))
            {
                animals.AddRange(db.Animals.Where(a => a.Name == updates[2]));
            }
            if (updates.ContainsKey(3))
            {
                animals.AddRange(db.Animals.Where(a => a.Age == Int32.Parse(updates[3])));
            }
            if (updates.ContainsKey(4))
            {
                animals.AddRange(db.Animals.Where(a => a.Demeanor == updates[4]));
            }
            if (updates.ContainsKey(5))
            {
                animals.AddRange(db.Animals.Where(a => a.KidFriendly == Boolean.Parse(updates[5])));
            }
            if (updates.ContainsKey(6))
            {
                animals.AddRange(db.Animals.Where(a => a.PetFriendly == Boolean.Parse(updates[6])));
            }
            if (updates.ContainsKey(7))
            {
                animals.AddRange(db.Animals.Where(a => a.Weight == Int32.Parse(updates[7])));
            }
            if (updates.ContainsKey(8))
            {
                animals.AddRange(db.Animals.Where(a => a.AnimalId == Int32.Parse(updates[8])));
            }

            return animals.Distinct().ToList();
        }

        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            Category categoryid = db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
            if (categoryid == null)
            {
                return -1;
            }
            return categoryid.CategoryId;
        }

        internal static Room GetRoom(int animalId)
        {
            Room room = db.Rooms.Where(r => r.AnimalId == animalId).FirstOrDefault();
            if (room == null)
            {
                UserInterface.NoRoomAssigned();
                return CreateNewRoom(animalId);
            }
            else return room;
        }

        internal static int GetDietPlanId(string dietPlanName)
        {
            DietPlan dietPlan = db.DietPlans.Where(d => d.Name == dietPlanName).FirstOrDefault();
            if (dietPlan == null)
            {
                return -1;
            }
            else return dietPlan.DietPlanId;
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            Adoption adopt = new Adoption();
            adopt.ClientId = client.ClientId;
            adopt.AnimalId = animal.AnimalId;
            adopt.ApprovalStatus = null;
            adopt.AdoptionFee = 25;
            adopt.PaymentCollected = false;

            db.Adoptions.InsertOnSubmit(adopt);
            db.SubmitChanges();
        }

        internal static List<Adoption> GetPendingAdoptions()
        {
            var pendingAdoptions = db.Adoptions.Where(a => a.ApprovalStatus == "unnaproved");
            return pendingAdoptions.ToList();
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            if (isAdopted == true)
            {
                adoption.ApprovalStatus = "approved";
            }
            else
            {
                adoption.ApprovalStatus = "unapproved";
            }

            db.SubmitChanges();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            Adoption deletingAdoption = db.Adoptions.Where(a => a.AnimalId == animalId && a.ClientId == clientId).FirstOrDefault();
            db.Adoptions.DeleteOnSubmit(deletingAdoption);
            db.SubmitChanges();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            throw new NotImplementedException();
        }

        //internal static Employee ReturnReadEmployee(Employee employeeFromDb, Employee employee)
        //{
        //    employeeFromDb = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
        //    return employeeFromDb;
        //}
        internal static Room CreateNewRoom(int animalID)
        {
            Room newRoom = new Room();
            newRoom.RoomId = UserInterface.GetIntegerData("ID", "the room's");
            newRoom.RoomNumber = UserInterface.GetIntegerData("number", "the room's");
            newRoom.AnimalId = animalID;
            return newRoom;
        }
    }
}