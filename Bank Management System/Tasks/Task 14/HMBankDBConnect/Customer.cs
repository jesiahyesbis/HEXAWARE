using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public string Name { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string Address { get; }

        public Customer(string name, string email, string phoneNumber, string address)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public Customer(long customerId, string name, string email, string phoneNumber, string address)
            : this(name, email, phoneNumber, address)
        {
            CustomerId = customerId;
        }

        public override string ToString()
        {
            return $"Customer ID: {CustomerId}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber}";
        }
    }

}
