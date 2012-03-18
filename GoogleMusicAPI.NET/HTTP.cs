using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace GoogleMusicAPI
{
    public class HTTP
    {
        public delegate void RequestCompletedEventHandler(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error);
        public delegate void RequestProgressEventHandler(HttpWebRequest request, int percentage);

        static HTTP()
        {

        }

        private class RequestState
        {
            public HttpWebRequest Request;
            public byte[] UploadData;
            public int MillisecondsTimeout;
            public RequestCompletedEventHandler CompletedCallback;
            public RequestProgressEventHandler ProgressCallback;

            public RequestState(HttpWebRequest request, byte[] uploadData, int millisecondsTimeout, RequestCompletedEventHandler completedCallback, RequestProgressEventHandler progressCallback)
            {
                Request = request;
                UploadData = uploadData;
                MillisecondsTimeout = millisecondsTimeout;
                CompletedCallback = completedCallback;
                ProgressCallback = progressCallback;
            }
        }

        public HttpWebRequest UploadDataAsync(Uri address, FormBuilder builder, RequestCompletedEventHandler complete, int timeout = 10000)
        {
            return UploadDataAsync(address, builder.ContentType, builder.GetBytes(), timeout, complete, null);
        }

        public HttpWebRequest UploadDataAsync(Uri address, string contentType, byte[] data, int millisecondsTimeout, RequestCompletedEventHandler completedCallback, RequestProgressEventHandler progressCallback)
        {
            // Create the request
            HttpWebRequest request = SetupRequest(address);
            
#if !NETFX_CORE
            request.ContentLength = (data != null) ? data.Length : 0;
#endif

            if (!String.IsNullOrEmpty(contentType))
                request.ContentType = contentType;

            request.Method = "POST";
            RequestState state = new RequestState(request, data, millisecondsTimeout, completedCallback, progressCallback);
            IAsyncResult result = request.BeginGetRequestStream(OpenWrite, state);

#if !NETFX_CORE
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, TimeoutCallback, state, millisecondsTimeout, true);
#endif

            return request;
        }


        public HttpWebRequest DownloadStringAsync(Uri address,  RequestCompletedEventHandler completedCallback, int millisecondsTimeout = 10000)
        {
            HttpWebRequest request = SetupRequest(address);
            request.Method = "GET";
            DownloadDataAsync(request, null, millisecondsTimeout, completedCallback);
            return request;
        }

        public void DownloadDataAsync(HttpWebRequest request, byte[] d,  int millisecondsTimeout,
           RequestCompletedEventHandler completedCallback)
        {
            RequestState state = new RequestState(request, d, millisecondsTimeout, completedCallback, null);

            IAsyncResult result = request.BeginGetResponse(GetResponse, state);

#if !NETFX_CORE
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, TimeoutCallback, state, millisecondsTimeout, true);
#endif
        }


        public virtual HttpWebRequest SetupRequest(Uri address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);

#if !NETFX_CORE
            request.ServicePoint.MaxIdleTime = 1000 * 60;
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.ConnectionLimit = 20;
            request.ServicePoint.UseNagleAlgorithm = false;
#endif

            return request;
        }

        void OpenWrite(IAsyncResult ar)
        {
            RequestState state = (RequestState)ar.AsyncState;

            try
            {
                // Get the stream to write our upload to
                using (Stream uploadStream = state.Request.EndGetRequestStream(ar))
                {
                    byte[] buffer = new Byte[checked((uint)Math.Min(1024, (int)state.UploadData.Length))];

                    MemoryStream ms = new MemoryStream(state.UploadData);

                    int bytesRead;
                    int i = 0;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        int prog = (int)Math.Floor(Math.Min(100.0,
                                (((double)(bytesRead * i) / (double)ms.Length) * 100.0)));


                        uploadStream.Write(buffer, 0, bytesRead);

                        i++;

                        if (state.ProgressCallback != null)
                            state.ProgressCallback(state.Request, prog);
                    }

                    if (state.ProgressCallback != null)
                        state.ProgressCallback(state.Request, 100);

#if !NETFX_CORE
                    ms.Close();
                    uploadStream.Close();
#endif
                }

                IAsyncResult result = state.Request.BeginGetResponse(GetResponse, state);

#if !NETFX_CORE
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, TimeoutCallback, state,
                    state.MillisecondsTimeout, true);
#endif

            }
            catch (Exception ex)
            {
                if (state.CompletedCallback != null)
                    state.CompletedCallback(state.Request, null, null, ex);
            }
        }

        void GetResponse(IAsyncResult ar)
        {
            RequestState state = (RequestState)ar.AsyncState;
            HttpWebResponse response = null;
            Exception error = null;
            String result = "";

            try
            {
                response = (HttpWebResponse)state.Request.EndGetResponse(ar);
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);

                    result = reader.ReadToEnd();

#if !NETFX_CORE
                    reader.Close();
                    responseStream.Close();
#endif

                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (state.CompletedCallback != null)
                state.CompletedCallback(state.Request, response, result, error);
        }

        void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                RequestState requestState = state as RequestState;
                if (requestState != null && requestState.Request != null)
                {
                    requestState.Request.Abort();
                }
            }
        }
    }
}
