using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Server.Services.DynamicNPC.Items;
using Server.Services.DynamicNPC.Data;

namespace Server.Services.DynamicNPC.Tasks
{
	static internal class TaskRewards
	{
		static internal ITaskReward GetProfRewards(string profession)
		{
			var interfaceType = typeof(ITaskReward);
			var types = Assembly.GetExecutingAssembly().GetExportedTypes()
				.Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

			var matchingInstances = new List<ITaskReward>();

			foreach (var type in types)
			{
				var property = type.GetProperty("VendorType");

				if (Enum.TryParse<VendorProfessions.VendorTypes>(property.GetValue(Activator.CreateInstance(type)).ToString(), out var enumValue))
				{
					if (VendorProfessions.IsGoodVendor(profession, out var prof) && enumValue == prof)
					{
						var instance = (ITaskReward)Activator.CreateInstance(type);

						matchingInstances.Add(instance);
					}
				}
			}

			if (matchingInstances.Count == 2)
			{
				var reward = Utility.RandomList(matchingInstances[0], matchingInstances[1]);

				return reward;
			}
			else
			{
				return null;
			}
		}
	}
}
