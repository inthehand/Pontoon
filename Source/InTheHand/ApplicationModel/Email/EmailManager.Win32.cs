//-----------------------------------------------------------------------
// <copyright file="EmailManager.Win32.cs" company="In The Hand Ltd">
//     Copyright © 2017 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace InTheHand.ApplicationModel.Email
{
    partial class EmailManager
    {
        private static void ShowComposeNewEmailImpl(EmailMessage message)
        {
            bool useAnsi = global::System.Environment.OSVersion.Version < new Version(6, 2);
            int recipCount = message.To.Count + message.CC.Count + message.Bcc.Count;

            NativeMethods.MapiMessage msg = new NativeMethods.MapiMessage();
            if (useAnsi)
            {
                if (!string.IsNullOrEmpty(message.Subject))
                    msg.lpszSubject = Marshal.StringToHGlobalAnsi(message.Subject);

                if (!string.IsNullOrEmpty(message.Body))
                    msg.lpszNoteText = Marshal.StringToHGlobalAnsi(message.Body);
            }
            else
            {
                if (!string.IsNullOrEmpty(message.Subject))
                    msg.lpszSubject = Marshal.StringToHGlobalUni(message.Subject);

                if (!string.IsNullOrEmpty(message.Body))
                    msg.lpszNoteText = Marshal.StringToHGlobalUni(message.Body);
            }

            // recipients
           
            if(recipCount > 0)
            {
                int recipdesclen = Marshal.SizeOf<NativeMethods.MapiRecipDesc>();
                msg.lpRecips = Marshal.AllocHGlobal(recipdesclen * recipCount);
                msg.nRecipCount = recipCount;
                int currentRecip = 0;

                foreach(EmailRecipient r in message.To)
                {
                    if (useAnsi)
                    {
                        var rd = new NativeMethods.MapiRecipDesc(r.Address, r.Name, NativeMethods.RecipientClass.MAPI_TO);
                        Marshal.StructureToPtr(rd, IntPtr.Add(msg.lpRecips, recipdesclen * currentRecip), false);
                    }
                    else
                    {
                        var rd = new NativeMethods.MapiRecipDescW(r.Address, r.Name, NativeMethods.RecipientClass.MAPI_TO);
                        Marshal.StructureToPtr(rd, IntPtr.Add(msg.lpRecips, recipdesclen * currentRecip), false);
                    }

                    currentRecip++;
                }

                foreach (EmailRecipient r in message.CC)
                {
                    if(useAnsi)
                    {
                        var rd = new NativeMethods.MapiRecipDesc(r.Address, r.Name, NativeMethods.RecipientClass.MAPI_CC);
                        Marshal.StructureToPtr(rd, IntPtr.Add(msg.lpRecips, recipdesclen * currentRecip), false);
                    }
                    else
                    {
                        var rd = new NativeMethods.MapiRecipDescW(r.Address, r.Name, NativeMethods.RecipientClass.MAPI_CC);
                        Marshal.StructureToPtr(rd, IntPtr.Add(msg.lpRecips, recipdesclen * currentRecip), false);
                    }

                    currentRecip++;
                }

                foreach (EmailRecipient r in message.Bcc)
                {
                    if (useAnsi)
                    {
                        var rd = new NativeMethods.MapiRecipDesc(r.Address, r.Name, NativeMethods.RecipientClass.MAPI_BCC);
                        Marshal.StructureToPtr(rd, IntPtr.Add(msg.lpRecips, recipdesclen * currentRecip), false);
                    }
                    else
                    {
                        var rd = new NativeMethods.MapiRecipDescW(r.Address, r.Name, NativeMethods.RecipientClass.MAPI_BCC);
                        Marshal.StructureToPtr(rd, IntPtr.Add(msg.lpRecips, recipdesclen * currentRecip), false);
                    }

                    currentRecip++;
                }
            }

            // attachments
            if(message.Attachments.Count > 0)
            {
                int fileDescLen = Marshal.SizeOf<NativeMethods.MapiFileDesc>();
                msg.lpFiles = Marshal.AllocHGlobal(fileDescLen * message.Attachments.Count);
                msg.nFileCount = message.Attachments.Count;

                for(int i = 0; i < message.Attachments.Count; i++)
                {
                    if (useAnsi)
                    {
                        NativeMethods.MapiFileDesc f = new Email.EmailManager.NativeMethods.MapiFileDesc(message.Attachments[i].Data.Path, message.Attachments[i].FileName);
                        Marshal.StructureToPtr(f, IntPtr.Add(msg.lpFiles, fileDescLen * i), false);
                    }
                    else
                    {
                        NativeMethods.MapiFileDescW f = new Email.EmailManager.NativeMethods.MapiFileDescW(message.Attachments[i].Data.Path, message.Attachments[i].FileName);
                        Marshal.StructureToPtr(f, IntPtr.Add(msg.lpFiles, fileDescLen * i), false);
                    }
                }
            }

            if (useAnsi)
            {
                uint result = NativeMethods.MAPISendMail(IntPtr.Zero, IntPtr.Zero, ref msg, 0xd, 0);
            }
            else
            {
                uint result = NativeMethods.MAPISendMailW(IntPtr.Zero, IntPtr.Zero, ref msg, 0xd, 0);
            }

            FreeMapiMessage(msg);
        }

        private static void FreeMapiMessage(NativeMethods.MapiMessage msg)
        {
            if(msg.lpszSubject != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(msg.lpszSubject);
                msg.lpszSubject = IntPtr.Zero;
            }

            if(msg.lpszNoteText != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(msg.lpszNoteText);
                msg.lpszNoteText = IntPtr.Zero;
            }

            if(msg.lpFiles != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(msg.lpFiles);
                msg.lpFiles = IntPtr.Zero;
                msg.nFileCount = 0;
            }

            if (msg.lpRecips != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(msg.lpRecips);
                msg.lpRecips = IntPtr.Zero;
                msg.nRecipCount = 0;
            }
        }

        private static class NativeMethods
        {
            [DllImport("Mapi32.dll", CharSet =CharSet.Ansi)]
            internal static extern uint MAPISendMail(IntPtr lhSession, IntPtr ulUIParam, ref MapiMessage lpMessage, int flFlags, uint ulReserved);

            // Windows 8 and above have the Unicode version which is preferred
            [DllImport("Mapi32.dll", CharSet = CharSet.Unicode)]
            internal static extern uint MAPISendMailW(IntPtr lhSession, IntPtr ulUIParam, ref MapiMessage lpMessage, int flFlags, uint ulReserved);

            [StructLayout(LayoutKind.Sequential)]
            internal struct MapiMessage
            {
                uint ulReserved;
                internal IntPtr lpszSubject;
                internal IntPtr lpszNoteText;
                IntPtr lpszMessageType;
                IntPtr lpszDateReceived;
                IntPtr lpszConversationID;
                int flFlags;
                IntPtr lpOriginator;
                internal int nRecipCount;
                internal IntPtr lpRecips;
                internal int nFileCount;
                internal IntPtr lpFiles;

            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            internal struct MapiFileDesc
            {
                uint ulReserved;
                uint flFlags;
                int nPosition;
                string lpszPathName;
                string lpszFileName;
                uint lpFileType;

                public MapiFileDesc(string path, string filename)
                {
                    ulReserved = 0;
                    flFlags = 0;
                    nPosition = -1;
                    lpszPathName = path;
                    lpszFileName = filename;
                    lpFileType = 0;
                }
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct MapiFileDescW
            {
                uint ulReserved;
                uint flFlags;
                int nPosition;
                string lpszPathName;
                string lpszFileName;
                uint lpFileType;

                public MapiFileDescW(string path, string filename)
                {
                    ulReserved = 0;
                    flFlags = 0;
                    nPosition = -1;
                    lpszPathName = path;
                    lpszFileName = filename;
                    lpFileType = 0;
                }
            }


            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            internal struct MapiRecipDesc
            {
                uint ulReserved;
                RecipientClass ulRecipClass;
                string lpszName;
                string lpszAddress;
                uint ulEIDSize;
                uint lpEntryID;

                public MapiRecipDesc(string address, string name, RecipientClass rc)
                {
                    ulReserved = 0;
                    ulRecipClass = rc;
                    lpszName = name;
                    lpszAddress = address;
                    ulEIDSize = 0;
                    lpEntryID = 0;
                }
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct MapiRecipDescW
            {
                uint ulReserved;
                RecipientClass ulRecipClass;
                string lpszName;
                string lpszAddress;
                uint ulEIDSize;
                uint lpEntryID;

                public MapiRecipDescW(string address, string name, RecipientClass rc)
                {
                    ulReserved = 0;
                    ulRecipClass = rc;
                    lpszName = name;
                    lpszAddress = address;
                    ulEIDSize = 0;
                    lpEntryID = 0;
                }
            }

            internal enum RecipientClass
            {
                /// <summary>
                /// Indicates the original sender of the message. 
                /// </summary>
                MAPI_ORIG = 0,

                /// <summary>
                /// Indicates a primary recipient of the message.
                /// </summary>
                MAPI_TO = 1,

                /// <summary>
                /// Indicates the recipient of a copy of the message.
                /// </summary>
                MAPI_CC = 2,

                /// <summary>
                /// Indicates the recipient of a blind copy of the message.
                /// </summary>
                MAPI_BCC = 3,
            }
        }      
    }
}