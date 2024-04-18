using System;
using System.Linq;
using Microsoft.Kinect;

namespace KinectExample
{
    class Program
    {
        static void Main(string[] args)
        {
            KinectSensor kinectSensor = KinectSensor.KinectSensors[0];

            kinectSensor.SkeletonFrameReady += KinectSensor_SkeletonFrameReady;

            kinectSensor.SkeletonStream.Enable();
            //kinectSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;

            Console.WriteLine("Rozpoczynamy śledzenie ruchu...");

            kinectSensor.Start();

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć.");

            Console.ReadKey();

            kinectSensor.Stop();
        }

        private static void KinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];

                    skeletonFrame.CopySkeletonDataTo(skeletons);

                    foreach (Skeleton skeleton in skeletons)
                    {
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            Joint rightHand = skeleton.Joints[JointType.HandRight];
                            Joint leftHand = skeleton.Joints[JointType.HandLeft];
                            Joint rightElbow = skeleton.Joints[JointType.ElbowRight];
                            Joint leftElbow = skeleton.Joints[JointType.ElbowLeft];
                            Joint rightShoulder = skeleton.Joints[JointType.ShoulderRight];
                            Joint leftShoulder = skeleton.Joints[JointType.ShoulderLeft];


                            bool isRightHandRaised = rightHand.Position.Y > rightElbow.Position.Y && rightElbow.Position.Y > rightShoulder.Position.Y;
                            bool isLeftHandRaised = leftHand.Position.Y > leftElbow.Position.Y && leftElbow.Position.Y > leftShoulder.Position.Y;

                            if (isRightHandRaised)
                            {
                                Console.WriteLine("Prawa dłoń podniesiona: X={0}, Y={1}, Z={2}", rightHand.Position.X, rightHand.Position.Y, rightHand.Position.Z);
                            }
                            if (isLeftHandRaised)
                            {
                                Console.WriteLine("Lewa dłoń podniesiona: X={0}, Y={1}, Z={2}", leftHand.Position.X, leftHand.Position.Y, leftHand.Position.Z);
                            }

                            Joint rightKnee = skeleton.Joints[JointType.KneeRight];
                            Joint leftKnee = skeleton.Joints[JointType.KneeLeft];
                            Joint rightHip = skeleton.Joints[JointType.HipRight];
                            Joint leftHip = skeleton.Joints[JointType.HipLeft];

                            bool isRightKneeRaised = rightKnee.Position.Y > rightHip.Position.Y;
                            bool isLeftKneeRaised = leftKnee.Position.Y > leftHip.Position.Y;

                            if (isRightKneeRaised)
                            {
                                Console.WriteLine("Prawa kolano podniesione: X={0}, Y={1}, Z={2}", rightKnee.Position.X, rightKnee.Position.Y, rightKnee.Position.Z);
                            }
                            if (isLeftKneeRaised)
                            {
                                Console.WriteLine("Lewa kolano podniesione: X={0}, Y={1}, Z={2}", leftKnee.Position.X, leftKnee.Position.Y, leftKnee.Position.Z);
                            }
                        }
                    }
                }
            }
        }
    }
}
