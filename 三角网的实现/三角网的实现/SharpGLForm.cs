using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;

namespace 三角网的实现
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        /// 
        private BuildTin Tin = new BuildTin();
        private Points centerpoint = new Points();
        public SharpGLForm()
        {
            try
            {
                InitializeComponent();
                //1、先获取数据
                Tin.GetPointData();
                Tin.init();
                centerpoint = Tin.CenterPoint;
            }
            catch { }
           

        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        /// 


        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            try
            {
                double LIGHT = 400;
                //  Get the OpenGL object.
                OpenGL gl = openGLControl.OpenGL;
                gl.LookAt(Tin.CenterPoint.X, Tin.CenterPoint.Y, -300, Tin.CenterPoint.X, Tin.CenterPoint.Y, 50, 0, 1, 0);

                //  Clear the color and depth buffer.
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                //  Load the identity matrix.
                gl.LoadIdentity();
                gl.ClearColor(0, 0.3f, 0, 1);



               
                gl.ShadeModel(OpenGL.GL_FLAT);
                SharpGL.SceneGraph.Assets.Texture texture = new SharpGL.SceneGraph.Assets.Texture();
                texture.Create(gl, @".\123.bmp");
                gl.Enable(OpenGL.GL_TEXTURE_2D);
                texture.Bind(gl);
                #region //添加纹理
                gl.Begin(OpenGL.GL_TRIANGLES);
                foreach (Triangle x in Tin.Triangledata)
                {
                    gl.TexCoord(0.0f, 0.0f); gl.Vertex(x.GetTriPointA.X, x.GetTriPointA.Y, x.GetTriPointA.Z);
                    gl.TexCoord(1f, 0f); gl.Vertex(x.GetTriPointB.X, x.GetTriPointB.Y, x.GetTriPointB.Z);
                    gl.TexCoord(1f, 1f); gl.Vertex(x.GetTriPointC.X, x.GetTriPointC.Y, x.GetTriPointC.Z);
                }
                gl.End();



                #endregion


                gl.Enable(OpenGL.GL_LIGHTING);
                gl.Enable(OpenGL.GL_LIGHT0);
                gl.Enable(OpenGL.GL_LIGHT1);
                gl.Enable(OpenGL.GL_DEPTH_TEST);
                gl.ShadeModel(OpenGL.GL_FLAT);
                gl.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_FALSE);


                Tin.Process();
                //绑定纹理
               


                #region//画三角网

                gl.LineWidth(1 / 2);
                //gl.Begin(OpenGL.GL_LINES);


                gl.Color(0, 0, 0);
                gl.Begin(OpenGL.GL_LINES);
                foreach (Edge temp1 in Tin.UsedEdgedata)
                {

                    gl.TexCoord(0.0f, 0.0f); gl.Vertex(temp1.GetPointa.X, temp1.GetPointa.Y, temp1.GetPointa.Z);
                    gl.TexCoord(1f, 1f); gl.Vertex(temp1.GetPointb.X, temp1.GetPointb.Y, temp1.GetPointb.Z);
                }
                foreach (Edge temp in Tin.OnlyEdgedata)
                {

                    gl.TexCoord(0.0f, 0.0f); gl.Vertex(temp.GetPointa.X, temp.GetPointa.Y, temp.GetPointa.Z);
                    gl.TexCoord(1f, 1f); gl.Vertex(temp.GetPointb.X, temp.GetPointb.Y, temp.GetPointb.Z);

                }

                gl.End();
                #endregion
                #region//添加光照
                gl.ShadeModel(OpenGL.GL_SMOOTH);
                //初始化材质
                var mat_specular = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
                var mat_ambient = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
                var mat_diffuse = new float[] { 2.0f, 2.0f, 2.0f, 0.1f };
                var mat_shininess = new float[] { 100.0f };
                gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SPECULAR, mat_specular);
                gl.Material(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT, mat_ambient);
                gl.Material(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE, mat_diffuse);
                gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, mat_shininess);
                //初始化光照
                var ambientLight = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
                var diffuseLight = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
                var posLight0 = new float[] { 2.0f, 0.1f, 0.0f, 0.0f };
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambientLight);
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, diffuseLight);
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, posLight0);
                #endregion
             





            }
            catch { }




        }




        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);

            
           
        
        
        }


        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  TODO: Set the projection matrix here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 1800.0);

           

            gl.LookAt(centerpoint.X-300 , centerpoint.Y+450 , centerpoint.Z +450, centerpoint.X, centerpoint.Y, centerpoint.Z, 0, 1, 0);
            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void 显示点位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Begin(OpenGL.GL_POINTS);
            foreach (Points s in Tin.Pointsdata)
            {
                gl.Vertex(s.X,s.Y,s.Z);
 
            }
            gl.End();

        }

        private void 显示三角网ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           


        }

        private void button1_Click(object sender, EventArgs e)
        {
           



        }

        /// <summary>
        /// The current rotation.
        /// </summary>



    }
}

