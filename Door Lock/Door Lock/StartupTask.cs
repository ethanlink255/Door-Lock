using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using System.Threading;
using System.IO;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Door_Lock
{
    public sealed class StartupTask : IBackgroundTask
    {
		public GpioController objGpio;
		public GpioPin objDoorStatusPin;

		public DoorStatus objDoorStatus;

		public WebService objWebService;

		public DoorControl objDoorController;

		
        public void Run(IBackgroundTaskInstance taskInstance)
        {
			// 
			// TODO: Insert code to perform background work
			//
			// If you start any asynchronous methods here, prevent the task
			// from closing prematurely by using BackgroundTaskDeferral as
			// described in http://aka.ms/backgroundtaskdeferral
			//

			InitGpio();
			InitWebService();
			InitDoorCheck();
			InitDoorController();

			while (true)
			{
				bool CheckForClosing = objWebService.DoorOpened;

				if (CheckForClosing) objDoorStatus.DoorOpened(objDoorStatusPin);

				objDoorController.CloseDoor();
				
			}

        }

		#region Init
		
		private void InitDoorController()
		{
			 objDoorController = new DoorControl();
		}

		private void InitDoorCheck()
		{
			objDoorStatus = new DoorStatus();
		}
		private void InitWebService()
		{
			objWebService = new WebService();
		}

		private void InitGpio()
		{
			objGpio = GpioController.GetDefault();

			if(objGpio == null)
			{
				throw new NullReferenceException();
		
			}

			//Pin number varies based on which one we use
			objDoorStatusPin = objGpio.OpenPin(3);
			objDoorStatusPin.SetDriveMode(GpioPinDriveMode.Input);
		}
		#endregion


	}

	public class DoorControl
	{



		public void OpenDoor()
		{
			throw new NotImplementedException();
		}
		public void CloseDoor()
		{
			throw new NotImplementedException();
		}
	}
	
	
	public class WebService
	{
		enum Clearance {FullAccess, SingleAccess, SingleAccessRestricted  };
		public Boolean DoorOpened = false;
		public void New()
		{
			while (!DoorOpened)
			{
				Check();
				
			}

		}

		public void Check()
		{
			//Not sure if just server or REST api class

		
	
			
			//if(requestRecieved){
			OpenRequestRecieved();
			//}


		}

		public void OpenRequestRecieved()
		{

			DoorControl objDoorController = new DoorControl();
			objDoorController.OpenDoor();
		
			DoorOpened = true;
			 

			
			
		}
	}


	public class DoorStatus
	 {

		private Boolean isOpen{ get; set; }
	

		internal void DoorOpened(GpioPin objDoorStatusPin)
		{
			isOpen = true;
			while (!isOpen)
			{
				isOpen = (objDoorStatusPin.Read() == GpioPinValue.Low);
			}

			

		}
	}
}
