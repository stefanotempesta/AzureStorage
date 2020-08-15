using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorage.Web.Models
{
    public class Queue : IEquatable<Queue>
    {
        public Queue() : this(null) { }

        public Queue(string name) => (Name, Messages) = (name, new List<QueueMessage>());

        [Required]
        public string Name { get; set; }

        public int Length => Messages.Count;

        public ICollection<QueueMessage> Messages { get; set; }

        public static Queue Null = new Queue();

        public bool Equals([AllowNull] Queue other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Name == other?.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Queue);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public static bool operator ==(Queue obj, Queue other)
        {
            return obj.Equals(other);
        }

        public static bool operator !=(Queue obj, Queue other)
        {
            return !(obj == other);
        }
    }
}
