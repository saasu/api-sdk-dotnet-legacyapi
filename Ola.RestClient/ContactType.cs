using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ola.RestClient
{
	[Flags]
	public enum ContactType
	{
		Employee = 1,
		Partner = 2,
		Customer = 4,
		Supplier = 8,
		User = 16,
		Contractor = 32
	}
}
