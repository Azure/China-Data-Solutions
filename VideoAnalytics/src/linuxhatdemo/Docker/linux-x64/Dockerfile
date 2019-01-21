# build builder image first
FROM microsoft/dotnet:2.1-runtime as builder
# install dependencies
RUN apt-get update
RUN apt-get install -y git unzip wget cmake libgtk2.0-dev libv4l-dev libavcodec-dev libavformat-dev libswscale-dev

# download opencv
RUN mkdir -p /opencv
RUN wget https://github.com/opencv/opencv/archive/3.4.1.zip
RUN unzip 3.4.1.zip -d /opencv/
RUN rm 3.4.1.zip
RUN wget https://github.com/opencv/opencv_contrib/archive/3.4.1.zip
RUN unzip 3.4.1.zip -d /opencv/
RUN rm 3.4.1.zip

RUN mkdir -p /opencv/build

WORKDIR /opencv/build

# install
RUN cmake -DOPENCV_EXTRA_MODULES_PATH=../opencv_contrib-3.4.1/modules -D WITH_LIBV4L=ON -D CMAKE_BUILD_TYPE=RELEASE -D WITH_TBB=ON -D ENABLE_NEON=ON ../opencv-3.4.1
RUN make -j 5
RUN make install
RUN ldconfig


# get opencvsharp
WORKDIR /
RUN wget https://github.com/shimat/opencvsharp/archive/3.3.1.20171117.zip
RUN unzip 3.3.1.20171117.zip -d /opencvsharp/

WORKDIR /opencvsharp/opencvsharp-3.3.1.20171117/src
RUN cmake .
RUN make -j 5
RUN make install


# build work image (multi-stage builds)
FROM microsoft/dotnet:2.1-runtime
ARG EXE_DIR=.
RUN mkdir /app

RUN apt-get update -y && \
    apt-get install -y libgtk2.0 libv4l-0 libavcodec57 libavformat57 libswscale4 && \
    rm -rf /var/lib/apt/lists/*


COPY --from=builder /usr/local /usr/local
COPY --from=builder /opencvsharp/opencvsharp-3.3.1.20171117/src/OpenCvSharpExtern/libOpenCvSharpExtern.so /app
RUN ldconfig

COPY $EXE_DIR/ /app

WORKDIR /app
CMD ["dotnet", "linuxhatdemo.dll"]