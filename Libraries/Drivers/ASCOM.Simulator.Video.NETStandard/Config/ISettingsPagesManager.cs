using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using xASCOM.Simulator.Properties;

namespace xASCOM.Simulator.Config
{
	public interface ISettingsPagesManager
	{
		void CameraTypeChanged(SimulatedCameraType cameraType);
	}
}
