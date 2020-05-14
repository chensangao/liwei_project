﻿using System;
using System.Runtime.InteropServices;
using System.IO;
using OpenCvSharp;

namespace testface
{
    // 人脸比较1:1、1:N、抽取人脸特征值、按特征值比较等
    class FaceCompare
    {
        // 提取人脸特征值(传图片文件路径)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_face_feature", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_face_feature(string file_name, ref int length);
        // 提取人脸特征值(传二进制图片buffer）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_face_feature_by_buf", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_face_feature_by_buf(byte[] buf,int size, ref int length);
        // 获取人脸特征值（传入opencv视频帧及人脸信息，适应于多人脸）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_face_feature_by_face", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_face_feature_by_face(IntPtr mat, ref TrackFaceInfo info, ref IntPtr feaptr);
        // 人脸1:1比对(传图片文件路径)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match(string file_name1, string file_name2);
        // 人脸1:1比对（传二进制图片buffer）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match_by_buf", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match_by_buf(byte[] buf1, int size1, byte[] buf2, int size2);
        // 人脸1:1比对（传opencv视频帧）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match_by_mat", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match_by_mat(IntPtr img1, IntPtr img2);// byte[] buf1, int size1, byte[] buf2, int size2);
        // 人脸1:1比对（传人脸特征值和二进制图片buffer)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match_by_feature", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match_by_feature(byte[] feature, int fea_len, byte[] buf2, int size2);
        // 特征值比对（传2个人脸的特征值）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "compare_feature", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern float compare_feature(byte[] f1, int f1_len, byte[] f2, int f2_len);
        // 1:N人脸识别（传图片文件路径和库里的比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify(string image, string group_id_list, string user_id, int user_top_num = 1);
        // 1:N人脸识别（传图片二进制文件buffer和库里的比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_buf", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_buf(byte[] buf, int size, string group_id_list,
            string user_id, int user_top_num = 1);
        // 1:N人脸识别（传人脸特征值和库里的比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_feature", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_feature(byte[] feature, int fea_len, string group_id_list, 
            string user_id, int user_top_num = 1);

        // 提前加载库里所有数据到内存中
        [DllImport("BaiduFaceApi.dll", EntryPoint = "load_db_face", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        public static extern bool load_db_face();

        // 1:N人脸识别（传人脸图片文件和内存已加载的整个库数据比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_with_all", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_with_all(string image, int user_top_num = 1);

        // 1:N人脸识别（传人脸图片文件和内存已加载的整个库数据比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_buf_with_all", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_buf_with_all(byte[] image, int size, int user_top_num = 1);

        // 1:N人脸识别（传人脸特征值和内存已加载的整个库数据比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_feature_with_all", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_feature_with_all(byte[] feature, int fea_len, int user_top_num = 1);

        // 测试获取人脸特征值(2048个byte）
        public void test_get_face_feature()
        {
            byte[] fea = new byte[2048];
            string file_name = "d:\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if(ptr==IntPtr.Zero)
            {
                Console.WriteLine("get face feature error");
            }
            else
            {
                if (len == 2048)
                {
                    Console.WriteLine("get face feature success");
                    Marshal.Copy(ptr, fea, 0, 2048);
                    // 可保存特征值2048个字节的fea到文件中
                   // FileUtil.byte2file("d:\\fea1.txt",fea, 2048);
                }
                else
                {
                    Console.WriteLine("get face feature error");
                }
            }
        }

        // 测试获取人脸特征值(2048个byte）
        public void test_get_face_feature_by_buf()
        {
            byte[] fea = new byte[2048];
            System.Drawing.Image img = System.Drawing.Image.FromFile("d:\\2.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            int len = 0;
            IntPtr ptr = get_face_feature_by_buf(img_bytes, img_bytes.Length, ref len);
            if (ptr == IntPtr.Zero)
            {
                Console.WriteLine("get face feature error");
            }
            else
            {
                if (len == 2048)
                {
                    Console.WriteLine("get face feature success");
                    Marshal.Copy(ptr, fea, 0, 2048);
                    // 可保存特征值2048个字节的fea到文件中
                    //  FileUtil.byte2file("d:\\fea2.txt",fea, 2048);
                }
                else
                {
                    Console.WriteLine("get face feature error");
                }
            }
        }
        // 测试1:1比较，传入图片文件路径
        public void test_match()
        {
            string file1 = "d:\\2.jpg";
            string file2 = "d:\\8.jpg";
            IntPtr ptr = match(file1, file2);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("match res is:" + buf);
        }
        // 测试1:1比较，传入图片文件二进制buffer
        public void test_match_by_buf()
        {
            System.Drawing.Image img1 = System.Drawing.Image.FromFile("d:\\444.bmp");
            byte[] img_bytes1 = ImageUtil.img2byte(img1);

            System.Drawing.Image img2 = System.Drawing.Image.FromFile("d:\\555.png");
            byte[] img_bytes2 = ImageUtil.img2byte(img2);
            Console.WriteLine("IntPtr ptr = match_by_buf");
            IntPtr ptr = match_by_buf(img_bytes1, img_bytes1.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("match_by_buf res is:" + buf);
        }
        // 测试1:1比较，传入opencv视频帧
        public void test_match_by_mat()
        {
            Mat img1 = Cv2.ImRead("d:\\444.bmp");
            Mat img2 = Cv2.ImRead("d:\\555.png");
            IntPtr ptr = match_by_mat(img1.CvPtr, img2.CvPtr);// img_bytes1, img_bytes1.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("match_by_buf res is:" + buf);
        }
        // 测试根据特征值和图片二进制buf比较
        public void test_match_by_feature()
        {
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            string file_name = "d:\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if( len!=2048)
            {
                Console.WriteLine("get face feature error!" );
                return;
            }
            Marshal.Copy(ptr, fea, 0, 2048);
            // 获取图片二进制buffer
            System.Drawing.Image img2 = System.Drawing.Image.FromFile("d:\\8.jpg");
            byte[] img_bytes2 = ImageUtil.img2byte(img2);

            IntPtr ptr_res = match_by_feature(fea, fea.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            Console.WriteLine("match_by_feature res is:" + buf);

        }

        // 测试1:N比较，传入图片文件路径
        public /*void*/string test_identify(string str, string usr_grp, string usr_id)
        {
            string file1 = str;//"d:\\6.jpg";
            string user_group = usr_grp;//"test_group";
            string user_id = usr_id;//"test_user";
            IntPtr ptr = identify(file1, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify res is:" + buf);
            return buf;
        }

        // 测试1:N比较，传入图片文件二进制buffer
        public void test_identify_by_buf(string str, string usr_grp, string usr_id)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(str);//"d:\\2.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);

            string user_group = usr_grp;//"test_group";
            string user_id = usr_id;// "test_user";
            IntPtr ptr = identify_by_buf(img_bytes, img_bytes.Length, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_by_buf res is:" + buf);
        }

        // 测试1:N比较，传入提取的人脸特征值
        public void test_identify_by_feature()
        {
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            string file_name = "d:\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (len != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr, fea, 0, 2048);

            string user_group = "test_group";
            string user_id = "test_user";
            IntPtr ptr_res = identify_by_feature(fea, fea.Length, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            Console.WriteLine("identify_by_feature res is:" + buf);
        }
        // 通过特征值比对（1:1）
        public void test_compare_feature()
        {
            // 获取特征值1   共2048个字节
            byte[] fea1 = new byte[2048];
            string file_name1 = "d:\\2.jpg";
            int len1 = 0;
            IntPtr ptr1 = get_face_feature(file_name1, ref len1);
            if (len1 != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr1, fea1, 0, 2048);

            // 获取特征值2   共2048个字节
            byte[] fea2 = new byte[2048];
            string file_name2 = "d:\\8.jpg";
            int len2 = 0;
            IntPtr ptr2 = get_face_feature(file_name2, ref len2);
            if (len2 != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr2, fea2, 0, 2048);
            // 比对
            float score = compare_feature(fea1, len1, fea2, len2);
            Console.WriteLine("compare_feature score is:"+score);
        }

        // 测试1:N比较，传入提取的人脸特征值和已加载的内存中整个库比较
        public void test_identify_by_feature_with_all()
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            string file_name = "d:\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (len != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr, fea, 0, 2048);
            IntPtr ptr_res = identify_by_feature_with_all(fea, fea.Length);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            Console.WriteLine("identify_by_feature_with_all res is:" + buf);
        }

        // 测试1:N比较，传入图片文件路径和已加载的内存中整个库比较
        public void test_identify_with_all()
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 1:N
            string file1 = "d:\\2.jpg";
            IntPtr ptr = identify_with_all(file1);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_with_all res is:" + buf);
        }

        // 测试1:N比较，传入图片文件二进制buffer和已加载的内存中整个库比较
        public void test_identify_by_buf_with_all()
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 1:N
            System.Drawing.Image img = System.Drawing.Image.FromFile("d:\\2.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            
            IntPtr ptr = identify_by_buf_with_all(img_bytes, img_bytes.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_by_buf_with_all res is:" + buf);
        }

    }
}
