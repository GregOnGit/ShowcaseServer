using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace ComplexWeb
{
    public class Program
    {

        public static void Main(string[] args)
        {
            AppConnect.StartListenAsync();

            // For normal Website
            CreateHostBuilder(args).Build().Run();

        }

        // For website
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    // Static cause I am lazy
    public class AppConnect
    {
        // The server that will listen
        static TcpListener s_server;

        // The information for a single client that will connect
        static TcpClient s_client;

        // The thread to run async tasks
        static Thread s_thread;

        // Acts as a mutex, so the program will only act on a connction that is fully established
        static private bool s_hasConnected = false;
        static public bool p_hasConnected{ get{ return s_hasConnected;} }

        // Used for checking disconnect
        static private bool s_stillConnected = false;

        // A buffer for sending commands to the client
        static private string s_stringToSend = "";
        
        // Sends a message to the client
        public static void SendMessageToClient( string l_msg )
        {
            // Append it on the end to the string to send
            s_stringToSend += l_msg;
        }

        public static void StartListenAsync()
        {
            // Listening on TCP port 8888
            s_server = new TcpListener( System.Net.IPAddress.Any, 8888);

            // Start the server
            s_server.Start();
            Console.WriteLine( "Server started. Waiting for connection..." );

            // Listen for Clients Async by doing on a new thread
            Thread s_thread = new Thread( new ThreadStart( ListenAndWait ) );

            // Name it and start it!
            s_thread.Name = "GregAsync";
            s_thread.Start();

        }

        // Meant to be run async
        // First allows the client to connect and then keeps listening for messages
        private static void ListenAndWait()
        {
            Console.WriteLine( "LISTENING YO" );

            // This is blocking, which is why it will be performed in a seperate thread
            s_client = s_server.AcceptTcpClient();

            // Has connected!
            s_hasConnected = true;
            s_stillConnected = true;

            while ( s_stillConnected )
            {
                // Checks if new data is available to be read from the network stream
                if( s_client.Available > 0 )
                {
                    // Create an array large enough to hold the recieved data
                    byte[] l_bytesRead = new byte[s_client.Available];

                    // Read the data from the network stream
                    s_client.GetStream().Read( l_bytesRead, 0, s_client.Available );

                    // Convert the byte array to a string we can understand
                    String l_str = System.Text.Encoding.ASCII.GetString( l_bytesRead );

                    // For now, just write whatever it is to console
                    Console.Write( l_str );
                }

                if( s_stringToSend != "" )
                {
                    // Once connected send "I have connected"
                    NetworkStream l_stream = s_client.GetStream();
                    byte[] l_bytesToSend = System.Text.Encoding.ASCII.GetBytes( s_stringToSend );
                    s_client.Client.Send( l_bytesToSend );

                    // Reset so there is no machine gun sounds
                    s_stringToSend = "";
                }

                // Suspend this whole thread for 50ms
                // We don't want to overload the server checking a quadrillion times a second
                Thread.Sleep( 50 );
            }
        }
    }
}