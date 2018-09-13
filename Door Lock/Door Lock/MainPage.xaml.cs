using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Door_Lock
{
	//This is part of a gui for debugging 

	//You can probably tell that I know nothing about async or UWP by reading this code

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		private GpioController objGpio;
		private GpioPin objDoorStatusPin;

		private DoorStatus objDoorStatus;

		private WebService objWebService;

		private DoorControl objDoorController;
		public MainPage()
        {
            this.InitializeComponent();

			InitGpio();
			InitWebService();
			InitDoorCheck();
			InitDoorController();


			//I looked into tasks due to the fact that I want the door checking to run asynchrously 
			//I don't know if the pi can handle threads though
			Task objTask = Task.Run(async () =>
		   {
			   while (true)
			   {
				   bool DoorOpened = objWebService.DoorOpened;
				   if (DoorOpened) {
					   objDoorStatus.DoorOpened(objDoorStatusPin);
					   DoorOpened = false;
					   objWebService.DoorOpened = false;

					   //Ensuring Fooly Closed Door
					   await Task.Delay(TimeSpan.FromSeconds(5));

					   objDoorController.CloseDoor();

				   }

			   }
		   });
			objTask.Wait();




		}

		

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

			if (objGpio == null)
			{
				throw new NullReferenceException();

			}

			//Pin number varies based on which one we use
			objDoorStatusPin = objGpio.OpenPin(3);
			objDoorStatusPin.SetDriveMode(GpioPinDriveMode.Input);
		}
	}

	public sealed class DoorControl
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


	public sealed class WebService
	{
		enum Clearance { FullAccess, SingleAccess, SingleAccessRestricted };
		public Boolean DoorOpened = false;

		public async void New()
		{
			while (true)
			{
				if (!DoorOpened) Check();
				//Reduces Number of Requests per day
				await Task.Delay(2000);

			}

		}

		public async void Check()
		{
			//Not sure if just server or REST api class

			WebRequest objRequest = WebRequest.Create("https://test-cea58.firebaseio.com/Requests.json");

			WebResponse objResponse = await objRequest.GetResponseAsync();




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






	public sealed class DoorStatus
	{
		private Boolean isOpen { get; set; }
		public void DoorOpened(GpioPin objDoorStatusPin)
		{
			isOpen = true;
			while (isOpen)
			{
				isOpen = (objDoorStatusPin.Read() == GpioPinValue.High);
			}
		}
	}
}




