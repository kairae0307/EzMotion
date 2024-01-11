using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport.Helpers;
using System.Collections;
using System.Diagnostics;


namespace EzMotion
{
    public partial class frmMove : Form
    {

      

        public int m_lAxisCounts = 0;                // 제어 가능한 축갯수 선언 및 초기화
        private int m_lAxisNo = 0;                // 제어할 축 번호 선언 및 초기화   
        public uint m_uModuleID = 0;                // 제어할 축의 모듈 I/O 선언 및 초기화
        public uint m_duOldResult = 0;                // tmTimer에서 사용할 이전 Command Position 선언 및 초기화
        String m_strResult;                           // 홈검색 결과를 출력 변수 선언
        public int m_lBoardNo = 0, m_lModulePos = 0;
        private static frmMain m_formMotion;
      //  private List<Form> tempForms;
     
        private bool[] m_bTestActive = new bool[64];

        private List<MotionValue> list_motions = new List<MotionValue>();

        public MotionValue m_MotionValue = new MotionValue();
        public Thread m_Thread = null;
        bool isThread = false;

        //콤보박스
        RepositoryItemComboBox rCombo = new RepositoryItemComboBox();
        //체크박스
        RepositoryItemCheckEdit rCheck = new RepositoryItemCheckEdit();
        //이미지
        RepositoryItemTextEdit rIMG = new RepositoryItemTextEdit();
        //토글스위치
        RepositoryItemToggleSwitch rToggle = new RepositoryItemToggleSwitch();


     
      //  public List<cl> list_motions = new List<Motion>();


        public frmMove()
        {
            InitializeComponent();

        }

        private void frmMove_Load(object sender, EventArgs e)
        {
           
            m_lAxisNo = cboSelAxis.SelectedIndex;
            tmDisplay.Enabled = false;
          //  tmHomeInfo.Enabled = false;
            initLibrary();      // 라이브러리 초기화 및 Mot파일을 불러옵니다.
            AddAxisInfo();      // 검색된 축 정보를 ComboBox에 등록/Control들을 초기화합니다.

        //    m_lAxisNo = cboSelAxis.SelectedIndex;       // 축 선택 ComboBox에서 선택된 축번호를 제어할 축 번호 변수에 설정합니다.
            //++ 지정한 축번호의 모듈ID를 반환합니다.
            // [INFO] 여러개의 정보를 읽는 함수 사용시 불필요한 정보는 NULL(0)을 입력하면 됩니다.
            CAXM.AxmInfoGetAxis(m_lAxisNo, ref m_lBoardNo, ref m_lModulePos, ref m_uModuleID);
          
            UpdateState();      // 모션보드의 상태와 Control들의 상태를 일치시킵니다.

           
           
         //  setGridStyles();
         //   gridControl1.DataSource = list_motions;
           // StartThread();
           
            //  RepositoryItemSetting();
        }


        public void StartThread()
        {
            if(m_Thread !=null)
            {
                Thread.Sleep(100);
                m_Thread.Abort();

            }
            isThread = true;
            m_Thread = new Thread(new ThreadStart(ThreadRun));
            m_Thread.Name = "Thread Pos Detect";
            m_Thread.IsBackground = true;
            m_Thread.Start();
        }

        private void ThreadRun()
        {
            double dCmdPosition = 0.0;
            double dActPosition = 0.0;
            double dCmdVelocity = 0.0;

            while (isThread)
            {

                /*
                long startTicks = DateTime.UtcNow.Ticks;
                // 측정할 코드
             
                //++ 지정한 축의 지령(Command)위치를 반환합니다.
                CAXM.AxmStatusGetCmdPos(m_lAxisNo, ref dCmdPosition);
                //++ 지정한 축의 실제(Feedback)위치를 반환합니다.
                CAXM.AxmStatusGetActPos(m_lAxisNo, ref dActPosition);
                //++ 지정한 축의 구동 속도를 반환합니다.
                CAXM.AxmStatusReadVel(m_lAxisNo, ref dCmdVelocity);

                /////////////////////////////////////////////////////////////////

                long endTicks = DateTime.UtcNow.Ticks;

                // 측정된 시간 출력
                long elapsedTicks = endTicks - startTicks;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                //  Console.WriteLine("측정된 시간: {0}ms", elapsedSpan.TotalMilliseconds);
                string strTemp = String.Format("측정된 시간: {0}ms", elapsedSpan.TotalMilliseconds);
                 
                Stopwatch stopwatch = new Stopwatch();

                // 시간 측정 시작
                stopwatch.Start();

                // 측정할 코드
                //++ 지정한 축의 지령(Command)위치를 반환합니다.
                CAXM.AxmStatusGetCmdPos(m_lAxisNo, ref dCmdPosition);
                //++ 지정한 축의 실제(Feedback)위치를 반환합니다.
                CAXM.AxmStatusGetActPos(m_lAxisNo, ref dActPosition);
                //++ 지정한 축의 구동 속도를 반환합니다.
                CAXM.AxmStatusReadVel(m_lAxisNo, ref dCmdVelocity);
                // 시간 측정 종료
                stopwatch.Stop();
                  string strTemp = String.Format("실행 시간: " + stopwatch.ElapsedMilliseconds + "ms");
               
                // 측정된 시간 출력
                // Console.WriteLine("실행 시간: " + stopwatch.ElapsedMilliseconds + "ms");

                */



                /*
                long StartTime, EndTime;
                Stopwatch SystemPerformanceWatch = new Stopwatch();
                SystemPerformanceWatch.Start();
                StartTime = SystemPerformanceWatch.ElapsedTicks;
                //성능 측정 코드 삽입 부분
                //  Thread.Sleep(1000);
                //++ 지정한 축의 지령(Command)위치를 반환합니다.
                CAXM.AxmStatusGetCmdPos(m_lAxisNo, ref dCmdPosition);
                //++ 지정한 축의 실제(Feedback)위치를 반환합니다.
                CAXM.AxmStatusGetActPos(m_lAxisNo, ref dActPosition);
                //++ 지정한 축의 구동 속도를 반환합니다.
                CAXM.AxmStatusReadVel(m_lAxisNo, ref dCmdVelocity);

                EndTime = SystemPerformanceWatch.ElapsedTicks;
            //    Console.WriteLine("연산소모 시간 {0}", String.Format("{0:#,###.###} ns", (1000 * 1000 * 1000 * (EndTime - StartTime) / Stopwatch.Frequency)));
                string strTemp = String.Format("시간 {0}", String.Format("{0:#,###.###} ns", (1000 * 1000 * 1000 * (EndTime - StartTime) / Stopwatch.Frequency)));
            
                richTextBox1.Invoke(new MethodInvoker(delegate () 
                {
                    richTextBox1.AppendText(strTemp + Environment.NewLine);
                    richTextBox1.ScrollToCaret();

                }));
                */


                /*
                uint duRetCode, duState = 0;
                uint duStepMain = 0, duStepSub = 0;

                //++ 지정한 축의 원점신호의 상태를 확인합니다.
                duRetCode = CAXM.AxmHomeReadSignal(m_lAxisNo, ref duState);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) chkHomeState.Checked = Convert.ToBoolean(duState);

                //++ 지정한 축의 원점신호의 상태를 확인합니다.
                CAXM.AxmHomeGetResult(m_lAxisNo, ref duState);
                if (m_duOldResult != duState)
                {
                    // labelHomeSearch.Text = TranslateHomeResult(duState);
                    labelHomeSearch.BeginInvoke(new Action(() => labelHomeSearch.Text = TranslateHomeResult(duState)));
                    m_duOldResult = duState;
                }
                //++ 지정한 축의 원점검색 결과를 확인합니다
                duRetCode = CAXM.AxmHomeGetRate(m_lAxisNo, ref duStepMain, ref duStepSub);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    // labelHomeStepMain32.Text = Convert.ToString(duStepMain);
                    // labelHomeStepSub33.Text = Convert.ToString(duStepSub);


                    labelHomeStepMain32.BeginInvoke(new Action(() => labelHomeStepMain32.Text = Convert.ToString(duStepMain)));
                    labelHomeStepSub33.BeginInvoke(new Action(() => labelHomeStepSub33.Text = Convert.ToString(duStepSub)));
                }
                //++ 지정한 축의 원점검색 진행율을 확인합니다.
                duRetCode = CAXM.AxmHomeGetRate(m_lAxisNo, ref duStepMain, ref duStepSub);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    // prgHomeRate.Value = (int)duStepSub;
                    prgHomeRate.BeginInvoke(new Action(() => prgHomeRate.Value = (int)duStepSub));
                }
                */



                //++ 지정한 축의 지령(Command)위치를 반환합니다.
                CAXM.AxmStatusGetCmdPos(m_lAxisNo, ref dCmdPosition);
                //++ 지정한 축의 실제(Feedback)위치를 반환합니다.
                CAXM.AxmStatusGetActPos(m_lAxisNo, ref dActPosition);
                //++ 지정한 축의 구동 속도를 반환합니다.
                CAXM.AxmStatusReadVel(m_lAxisNo, ref dCmdVelocity);

                textCmdPos.BeginInvoke(new Action(() => textCmdPos.Text = String.Format("{0:0.000}", dCmdPosition)));
                  textActPos.BeginInvoke(new Action(() => textActPos.Text = String.Format("{0:0.000}", dActPosition)));
                  textCmdVel.BeginInvoke(new Action(() => textCmdVel.Text = String.Format("{0:0.000}", dCmdVelocity)));

                m_MotionValue.CommandPotion = String.Format("{0:0.000}", dCmdPosition);
                m_MotionValue.ActualPotion = String.Format("{0:0.000}", dActPosition);
                m_MotionValue.CommandVelocity = String.Format("{0:0.000}", dCmdVelocity);


             



            

                //  if (textCmdPos.InvokeRequired)
                //  {
                // 작업쓰레드인 경우
                //  textCmdPos.BeginInvoke(new Action(() => textCmdPos.Text = String.Format("{0:0.000}", dCmdPosition)));
                //  textActPos.BeginInvoke(new Action(() => textActPos.Text = String.Format("{0:0.000}", dActPosition)));
                //  textCmdVel.BeginInvoke(new Action(() => textCmdVel.Text = String.Format("{0:0.000}", dCmdVelocity)));
                //  }

                //  string strTemp = String.Format("CmdPos : {0:0.000}\n", dCmdPosition) + String.Format("ActPos : {0:0.000}\n", dActPosition) + String.Format("CdmVel : {0:0.000}\n", dCmdVelocity);






                
                CheckBox[] SignalInput = new CheckBox[5];
                SignalInput[0] = chkInp00;
                SignalInput[1] = chkInp01; 
                SignalInput[2] = chkInp02;
                SignalInput[3] = chkInp03; 
                SignalInput[4] = chkInp04;

                CheckBox[] SignalOutput = new CheckBox[5];
                SignalOutput[0] = chkOut00; 
                SignalOutput[1] = chkOut01;
                SignalOutput[2] = chkOut02;
                SignalOutput[3] = chkOut03; 
                SignalOutput[4] = chkOut04;


                uint uBitNo = 0;
                uint duState1 = 0, duState2 = 0, duOnOff=0, duRetCode=0;
                uint duLevel1 = 0;


                //++ 지정한 축의 서보온 상태를 반환합니다.
                duRetCode= CAXM.AxmSignalIsServoOn(m_lAxisNo, ref duOnOff);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    m_MotionValue.ServoOn = Convert.ToBoolean(duOnOff);
                }
                    //++ Z상 신호의 상태를 확인합니다.
                    duRetCode = CAXM.AxmStatusReadMechanical(m_lAxisNo, ref duState1);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    duState1 = ((duState1 & (uint)AXT_MOTION_QIMECHANICAL_SIGNAL.QIMECHANICAL_ZPHASE_LEVEL) == 0) ? (uint)0 : (uint)1;
                    //chkZPhase.Checked = Convert.ToBoolean(duState1);
                }

                //++ Servo Alarm 신호의 상태를 확인합니다.
                duRetCode = CAXM.AxmSignalReadServoAlarm(m_lAxisNo, ref duState1);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //  chkAlarm.Checked = Convert.ToBoolean(duState1);
                    m_MotionValue.Alam = Convert.ToBoolean(duState1);
                }

                //++ Inposition(위치결정완료) 신호의 상태를 확인합니다.
                duRetCode = CAXM.AxmSignalReadInpos(m_lAxisNo, ref duState1);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                   // chkInp.Checked = Convert.ToBoolean(duState1);
                    m_MotionValue.InPotion = Convert.ToBoolean(duState1);
                }

                //++ Emergency 신호의 상태를 확인합니다.
                duRetCode = CAXM.AxmSignalReadStop(m_lAxisNo, ref duState1);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                   // chkEmg.Checked = Convert.ToBoolean(duState1);
                }

                //++ Emergency 신호의 상태를 확인합니다.
                duRetCode = CAXM.AxmStatusReadInMotion(m_lAxisNo, ref duState1);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    // chkEmg.Checked = Convert.ToBoolean(duState1);
                    m_MotionValue.InMotion = Convert.ToBoolean(duState1);
                }

             

                //++ (+/-)End Limit신호의 상태를 확인합니다.
                duRetCode = CAXM.AxmSignalReadLimit(m_lAxisNo, ref duState1, ref duState2);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //chkElimitP.Checked = Convert.ToBoolean(duState1);
                    //chkElimitN.Checked = Convert.ToBoolean(duState2);
                    m_MotionValue.LimitP = Convert.ToBoolean(duState1);
                    m_MotionValue.LimitN = Convert.ToBoolean(duState2);
                }
                //++ (+/-)Software Limit의 Active 상태를 확인합니다.
                duRetCode = CAXM.AxmSignalReadSoftLimit(m_lAxisNo, ref duState1, ref duState2);
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                   // chkSwlimitP.Checked = Convert.ToBoolean(duState1);
                   // chkSwlimitN.Checked = Convert.ToBoolean(duState2);
                }

                for (uBitNo = 0; uBitNo < (uint)AXT_MOTION_UNIV_OUTPUT.UIO_OUT5; uBitNo++)
                {
                    //++ 범용입력 신호의(Bit00-Bit04) 상태를 확인합니다.
                  //  duRetCode = CAXM.AxmSignalReadInputBit(m_lAxisNo, (int)uBitNo, ref duState1);

                  //  if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                  //      SignalInput[uBitNo].Checked = Convert.ToBoolean(duState1);

                    //++ 범용출력 신호의(Bit00-Bit04) 상태를 확인합니다.
                   // duRetCode = CAXM.AxmSignalReadOutputBit(m_lAxisNo, (int)uBitNo, ref duState2);
                  //  if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                     //   m_MotionValue.ServoOn = Convert.ToBoolean(duState2);
                    //  SignalOutput[uBitNo].Checked = Convert.ToBoolean(duState2);
                }

                m_MotionValue.Name = list_motions[m_lAxisNo].Name;
                m_MotionValue.No = list_motions[m_lAxisNo].No;
                list_motions[m_lAxisNo] = m_MotionValue;


                gridControl1.BeginInvoke(new Action(() => {
                    gridControl1.DataSource = list_motions;
                    gridControl1.Update();
                    //   gridView1.UpdateCurrentRow();
                }));

                Task.Run(() =>
                {
                    // gridView1.UpdateCurrentRow();
                    gridView1.RefreshData();

                }).Wait();
            }
}
        private void RepositoryItemSetting()
        {
            /*콤보박스*/
            //입력 방지
            rCombo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //목록 초기화
            rCombo.Items.Clear();
            //목록 추가
            rCombo.Items.AddRange(new string[] { "L1", "L2", "L3" });
            gridControl1.RepositoryItems.Add(rCombo);
            //초기값 선택
            gridView1.SetRowCellValue(1, "DT2", rCombo.Items[0]);

            /*체크박스*/
            //체크 구분 문자
            rCheck.ValueChecked = "Y";
            rCheck.ValueUnchecked = "N";
            //체크 스타일
            rCheck.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Standard;
            //라디오는 스타일을 바꿔줌
            //rCheck.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            rCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            //초기값 선택
            gridView1.SetRowCellValue(0, "DT1", "N");
            gridView1.SetRowCellValue(1, "DT1", "Y");
            gridView1.SetRowCellValue(2, "DT1", "N");
            gridView1.SetRowCellValue(3, "DT1", "Y");

            /*이미지*/
            //표시할 이미지
            rIMG.ContextImageOptions.Image = Image.FromFile(@"D:\KTK_WORK\Test\EzMotion\EzMotion\Resources\targetValue1big.png");
            //정렬(가운데 정렬은 어디에?)
            rIMG.ContextImageOptions.Alignment = DevExpress.XtraEditors.ContextImageAlignment.Near;
            //입력 방지
            rIMG.ReadOnly = true;

            /*토글스위치*/
            //토글 구분 문자
            rToggle.ValueOn = "Y";
            rToggle.ValueOff = "N";
            //초기값 선택
            gridView1.SetRowCellValue(3, "DT3", "N");
        }
     


        // ++ =======================================================================
        // >> InitLibrary() : 라이브러리 초기화 및 Mot파일을 불러오는 함수.
        //  - AXL(AjineXtek Library)을 사용가능하게 합니다. 
        //  - EzConfig 프로그램이 설치되어있는 폴더에서 기본 Mot파일(모션 설정파일)을
        //    지정해 모션관련 기본설정을 자동으로 합니다.
        // --------------------------------------------------------------------------
        public void initLibrary()
        {

            // ※ [CAUTION] 아래와 다른 Mot파일(모션 설정파일)을 사용할 경우 경로를 변경하십시요.
            //  String szFilePath = "C:\\Program Files\\EzSoftware RM\\EzSoftware\\MotionDefault.mot";
            //  String szFilePath = System.Windows.Forms.Application.StartupPath+"\\test.mot";
            String szFilePath = "C:\\test.mot";

            //++ AXL(AjineXtek Library)을 사용가능하게 하고 장착된 보드들을 초기화합니다.
            if (CAXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show("Intialize Fail..!!");


            //++ 지정한 Mot파일의 설정값들로 모션보드의 설정값들을 일괄변경 적용합니다.
            if (CAXM.AxmMotLoadParaAll(szFilePath) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show("Mot File Not Found.");
        }



        // **************************************************************************
        // >> [구분]: 별도의 기능을 구현하기위해 만든 사용자 정의 함수들 모음 
        // **************************************************************************

        // ++ =======================================================================
        // >> InitLibrary() : 라이브러리 초기화 및 Mot파일을 불러오는 함수.
        //  - AXL(AjineXtek Library)을 사용가능하게 합니다. 
        //  - EzConfig 프로그램이 설치되어있는 폴더에서 기본 Mot파일(모션 설정파일)을
        //    지정해 모션관련 기본설정을 자동으로 합니다.
        // --------------------------------------------------------------------------
        public void InitControl()
        {

          //  cboSelAxis.SelectedIndex = m_lAxisNo;
            // 원점검색 진행율을 표시할 프로그래스바 설정
            prgHomeRate.Step = 1;

            //  edtPos1.Value = Convert.ToDecimal("0.000");
            //  edtPos2.Value = Convert.ToDecimal("100.000");
            // edtMoveVel.Value = Convert.ToDecimal("100.000");
            //  edtMoveAcc.Value = Convert.ToDecimal("400.000");
            //  edtMoveDec.Value = Convert.ToDecimal("400.000");
            edtJogVel.Value = Convert.ToDecimal("2000.000");
            edtJogAcc.Value = Convert.ToDecimal("2000.000");
            edtJogDec.Value = Convert.ToDecimal("2000.000");
        }

        // ++ =======================================================================
        // >> AddAxisInfo() : 시스템에 장착된 모션축들을 ComboBox에 등록하는 함수.
        //  - AXL Open시 검색된 축들의 정보를 읽어 ComboBox에 등록하여 사용자가 선택
        //    제어할 수 있도록 합니다.
        //  - 유효한 축이 검색되었을 때 Control들을 초기화 하고, 정보 갱신용 타이머를 
        //    100msec 주기로 동작시킵니다.
        // --------------------------------------------------------------------------
        public void AddAxisInfo()
        {
            String strAxis = "";

            //++ 유효한 전체 모션축수를 반환합니다.
            CAXM.AxmInfoGetAxisCount(ref m_lAxisCounts);

           
            m_lAxisNo = 0;
            //++ 지정한 축의 정보를 반환합니다.
            // [INFO] 여러개의 정보를 읽는 함수 사용시 불필요한 정보는 NULL(0)을 입력하면 됩니다.
            CAXM.AxmInfoGetAxis(m_lAxisNo, ref m_lBoardNo, ref m_lModulePos, ref m_uModuleID);
            for (int i = 0; i < m_lAxisCounts; i++)
            {
                switch (m_uModuleID)
                {
                    //++ 지정한 축의 정보를 반환합니다.
                    // [INFO] 여러개의 정보를 읽는 함수 사용시 불필요한 정보는 NULL(0)을 입력하면 됩니다.
                    case (uint)AXT_MODULE.AXT_SMC_4V04: strAxis = String.Format("{0:00}-[PCIB-QI4A]", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_2V04: strAxis = String.Format("{0:00}-[PCIB-QI2A]", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04: strAxis = String.Format("{0:00}-(RTEX-PM)", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04PM2Q: strAxis = String.Format("{0:00}-(RTEX-PM2Q)", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04PM2QE: strAxis = String.Format("{0:00}-(RTEX-PM2QE)", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04A4: strAxis = String.Format("{0:00}-[RTEX-A4N]", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04A5: strAxis = String.Format("{0:00}-[RTEX-A5N]", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04MLIISV: strAxis = String.Format("{0:00}-[MLII-SGDV]", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04MLIIPM: strAxis = String.Format("{0:00}-(MLII-PM)", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04MLIICL: strAxis = String.Format("{0:00}-[MLII-CSDL]", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04MLIICR: strAxis = String.Format("{0:00}-[MLII-CSDH]", i); break;
                    case (uint)AXT_MODULE.AXT_SMC_R1V04SIIIHMIV: strAxis = String.Format("{0:00}-[SIIIH-MRJ4]", i); break;
                    default: strAxis = String.Format("{0:00}-[Unknown]", i); break;
                }
                InitControl();
                cboSelAxis.Items.Add(strAxis);
                MotionValue motion_value = new MotionValue();
                motion_value.No = String.Format("{0:000}", i);
                motion_value.Name = strAxis;
                list_motions.Add(motion_value);

            }
           // if (cboSelAxis.Items.Count !=0)
          //  {
          //      InitControl();      // Control 변수들을 등록하고, 초기 설정값들을 설정합니다.
           // }
       
        }
        // ++ =======================================================================
        // >> UpdateState() : 지정한 축의 설정값들을 읽어 Control과 일치시키는 함수.
        //  - 축번호가 변경되었을 때 변경한 축의 설정값들과 Control들과 일치시키는 
        //    역할을하는 함수입니다.
        //  - 지정한 축의 모델별 제약들을 Control에 반영하는 역할을 합니다.
        //  - 프로그램 초기 실행시, Mot파일 불러올 때, 축 번호 바뀔 때 사용됩니다.
        // --------------------------------------------------------------------------
        public void UpdateState()
        {
            int iHomeDir = 0;
            uint lHomeDir = 0;
            uint duRetCode, duLevel = 0, duOnOff = 0;
            uint duSWEnable = 0, duSWStopMode = 0, duSWRef = 0;
            uint duHomeSignal = 0, duZPhaseUse = 0;
            double dHomeClrTime = 0.0, dHomeOffset = 0.0;
            uint duLevel1 = 0, duLevel2 = 0,  duStopMode = 0;
            double dVelFirst = 100.0, dVelSecond = 50.0, dVelThird = 5.0, dVelLast = 1.0, dAccFirst = 400.0, dAccSecond = 100.0;
            double dSWLimitP = 0.0, dSWLimitN = 0.0;

            // edtSetPos.Text = "0.000";
            edtMovePos.Text = "40.000";
            edtMoveVel.Text = "5.000";
            edtMoveAcc.Text = "1.000";
            edtMoveDec.Text = "1.000";
            // edtOverrideDis1.Text = "2000.000";
            //  edtOverrideDis2.Text = "1000.000";


          
          






            // 각 제품별 지원여부에 따라 콘트롤 값들을 변경합니다.
            switch (m_uModuleID)
            {
                case (uint)AXT_MODULE.AXT_SMC_2V04:
                case (uint)AXT_MODULE.AXT_SMC_4V04:
                case (uint)AXT_MODULE.AXT_SMC_R1V04:
                case (uint)AXT_MODULE.AXT_SMC_R1V04PM2Q:
                case (uint)AXT_MODULE.AXT_SMC_R1V04PM2QE:
                    cboHomeLevel.Items.Clear();
                    cboHomeLevel.Enabled = true;
                    cboHomeLevel.Items.Add("00: LOW");
                    cboHomeLevel.Items.Add("01: HIGH");

                    cboHomeSignal.Items.Clear();
                    cboHomeSignal.Enabled = true;
                    cboHomeSignal.Items.Add("00: PosEndLimit");
                    cboHomeSignal.Items.Add("01: NegEndLimit");
                    cboHomeSignal.Items.Add("04: HomeSensor");
                    cboHomeSignal.Items.Add("05: EncodZPhase");

                    cboZphaseUse.Items.Clear();
                    cboZphaseUse.Enabled = true;
                    cboZphaseUse.Items.Add("00: DISABLE");
                    cboZphaseUse.Items.Add("01: DIR_CHOME");
                    cboZphaseUse.Items.Add("02: DIR_HOME");

                    cboZPhase.Items.Clear();
                    cboZPhase.Items.Add("00: LOW");
                    cboZPhase.Items.Add("01: HIGH");
                    cboAlarm.Items.Clear();
                    cboAlarm.Items.Add("00: LOW");
                    cboAlarm.Items.Add("01: HIGH");
                    cboAlarm.Items.Add("02: UNUSED");
                    cboInp.Items.Clear();
                    cboInp.Items.Add("00: LOW");
                    cboInp.Items.Add("01: HIGH");
                    cboInp.Items.Add("02: UNUSED");
                    cboEmg.Items.Clear();
                    cboEmg.Items.Add("00: LOW");
                    cboEmg.Items.Add("01: HIGH");
                    cboEmg.Items.Add("02: UNUSED");
                    cboElimitN.Items.Clear();
                    cboElimitN.Items.Add("00: LOW");
                    cboElimitN.Items.Add("01: HIGH");
                    cboElimitN.Items.Add("02: UNUSED");
                    cboElimitP.Items.Clear();
                    cboElimitP.Items.Add("00: LOW");
                    cboElimitP.Items.Add("01: HIGH");
                    cboElimitP.Items.Add("02: UNUSED");
                    cboServoOn.Items.Clear();
                    cboServoOn.Items.Add("00: LOW");
                    cboServoOn.Items.Add("01: HIGH");
                    cboAlarmReset.Items.Clear();
                    cboAlarmReset.Items.Add("00: LOW");
                    cboAlarmReset.Items.Add("01: HIGH");
                    cboSwRef.Items.Clear();
                    cboSwRef.Items.Add("00: COMMAND");
                    cboSwRef.Items.Add("01: ACTUAL");
                    cboSwStop.Items.Clear();
                    cboSwStop.Items.Add("00: E_STOP");
                    cboSwStop.Items.Add("01: S_STOP");
                    break;

                case (uint)AXT_MODULE.AXT_SMC_R1V04A4:
                    cboHomeLevel.Items.Clear();
                    cboHomeLevel.Enabled = true;
                    cboHomeLevel.Items.Add("01: HIGH");
                    cboHomeLevel.Items.Add("02: UNUSED");

                    cboHomeSignal.Items.Clear();
                    cboHomeSignal.Enabled = true;
                    cboHomeSignal.Items.Add("00: PosEndLimit");
                    cboHomeSignal.Items.Add("01: NegEndLimit");
                    cboHomeSignal.Items.Add("04: HomeSensor");

                    cboZphaseUse.Items.Clear();
                    cboZphaseUse.Enabled = true;
                    cboZphaseUse.Items.Add("00: DISABLE");
                    cboZphaseUse.Items.Add("01: DIR_CHOME");
                    cboZphaseUse.Items.Add("02: DIR_HOME");

                    cboZPhase.Items.Clear();
                    cboZPhase.Items.Add("02: UNUSED");
                    cboAlarm.Items.Clear();
                    cboAlarm.Items.Add("01: HIGH");
                    cboAlarm.Items.Add("02: UNUSED");
                    cboInp.Items.Clear();
                    cboInp.Items.Add("01: HIGH");
                    cboInp.Items.Add("02: UNUSED");
                    cboEmg.Items.Clear();
                    cboEmg.Items.Add("00: LOW");
                    cboEmg.Items.Add("02: UNUSED");
                    cboElimitN.Items.Clear();
                    cboElimitN.Items.Add("00: LOW");
                    cboElimitN.Items.Add("02: UNUSED");
                    cboElimitP.Items.Clear();
                    cboElimitP.Items.Add("00: LOW");
                    cboElimitP.Items.Add("02: UNUSED");
                    cboServoOn.Items.Clear();
                    cboServoOn.Items.Add("01: HIGH");
                    cboAlarmReset.Items.Clear();
                    cboAlarmReset.Items.Add("01: HIGH");
                    cboSwRef.Items.Clear();
                    cboSwRef.Items.Add("02: UNUSED");
                    cboSwStop.Items.Clear();
                    cboSwStop.Items.Add("02: UNUSED");
                    break;

                case (uint)AXT_MODULE.AXT_SMC_R1V04MLIIPM:
                    cboHomeLevel.Items.Clear();
                    cboHomeLevel.Enabled = false;
                    cboHomeLevel.Items.Add("01: HIGH");
                    cboHomeLevel.Items.Add("02: UNUSED");

                    cboZphaseUse.Items.Clear();
                    cboZphaseUse.Enabled = false;
                    cboZphaseUse.Items.Add("00: DISABLE");

                    cboHomeSignal.Items.Clear();
                    cboHomeSignal.Enabled = false;
                    cboHomeSignal.Items.Add("04: HomeSensor");

                    cboZPhase.Items.Clear();
                    cboZPhase.Items.Add("02: UNUSED");
                    cboAlarm.Items.Clear();
                    cboAlarm.Items.Add("02: UNUSED");
                    cboInp.Items.Clear();
                    cboInp.Items.Add("02: UNUSED");
                    cboEmg.Items.Clear();
                    cboEmg.Items.Add("02: UNUSED");
                    cboElimitN.Items.Clear();
                    cboElimitN.Items.Add("02: UNUSED");
                    cboElimitP.Items.Clear();
                    cboElimitP.Items.Add("02: UNUSED");
                    cboServoOn.Items.Clear();
                    cboServoOn.Items.Add("01: HIGH");
                    cboAlarmReset.Items.Clear();
                    cboAlarmReset.Items.Add("01: HIGH");
                    cboSwRef.Items.Clear();
                    cboSwRef.Items.Add("02: UNUSED");
                    cboSwStop.Items.Clear();
                    cboSwStop.Items.Add("02: UNUSED");
                    break;

                case (uint)AXT_MODULE.AXT_SMC_R1V04MLIISV:
                case (uint)AXT_MODULE.AXT_SMC_R1V04SIIIHMIV:
                    cboHomeLevel.Items.Clear();
                    cboHomeLevel.Enabled = false;
                    cboHomeLevel.Items.Add("01: HIGH");
                    cboHomeLevel.Items.Add("02: UNUSED");

                    cboHomeSignal.Items.Clear();
                    cboHomeSignal.Enabled = true;
                    cboHomeSignal.Items.Add("00: PosEndLimit");
                    cboHomeSignal.Items.Add("01: NegEndLimit");
                    cboHomeSignal.Items.Add("04: HomeSensor");

                    cboZphaseUse.Items.Clear();
                    cboZphaseUse.Enabled = true;
                    cboZphaseUse.Items.Add("00: DISABLE");
                    cboZphaseUse.Items.Add("01: DIR_CHOME");
                    cboZphaseUse.Items.Add("02: DIR_HOME");

                    cboZPhase.Items.Clear();
                    cboZPhase.Items.Add("02: UNUSED");
                    cboAlarm.Items.Clear();
                    cboAlarm.Items.Add("01: HIGH");
                    cboAlarm.Items.Add("02: UNUSED");
                    cboInp.Items.Clear();
                    cboInp.Items.Add("01: HIGH");
                    cboInp.Items.Add("02: UNUSED");
                    cboEmg.Items.Clear();
                    cboEmg.Items.Add("02: UNUSED");
                    cboElimitN.Items.Clear();
                    cboElimitN.Items.Add("01: HIGH");
                    cboElimitN.Items.Add("02: UNUSED");
                    cboElimitP.Items.Clear();
                    cboElimitP.Items.Add("01: HIGH");
                    cboElimitP.Items.Add("02: UNUSED");
                    cboServoOn.Items.Clear();
                    cboServoOn.Items.Add("01: HIGH");
                    cboAlarmReset.Items.Clear();
                    cboAlarmReset.Items.Add("01: HIGH");
                    cboSwRef.Items.Clear();
                    cboSwRef.Items.Add("02: UNUSED");
                    cboSwStop.Items.Clear();
                    cboSwStop.Items.Add("02: UNUSED");
                    break;

                case (uint)AXT_MODULE.AXT_SMC_R1V04MLIICL:
                case (uint)AXT_MODULE.AXT_SMC_R1V04MLIICR:
                    cboHomeSignal.Items.Clear();
                    cboHomeSignal.Enabled = true;
                    cboHomeSignal.Items.Add("00: PosEndLimit");
                    cboHomeSignal.Items.Add("01: NegEndLimit");
                    cboHomeSignal.Items.Add("04: HomeSensor");

                    cboZphaseUse.Items.Clear();
                    cboZphaseUse.Enabled = true;
                    cboZphaseUse.Items.Add("00: DISABLE");
                    cboZphaseUse.Items.Add("01: DIR_CHOME");
                    cboZphaseUse.Items.Add("02: DIR_HOME");

                    cboAlarm.Items.Clear();
                    cboAlarm.Items.Add("01: HIGH");
                    cboAlarm.Items.Add("02: UNUSED");
                    cboInp.Items.Clear();
                    cboInp.Items.Add("01: HIGH");
                    cboInp.Items.Add("02: UNUSED");
                    cboEmg.Items.Clear();
                    cboEmg.Items.Add("02: UNUSED");
                    cboElimitN.Items.Clear();
                    cboElimitN.Items.Add("01: HIGH");
                    cboElimitN.Items.Add("02: UNUSED");
                    cboElimitP.Items.Clear();
                    cboElimitP.Items.Add("01: HIGH");
                    cboElimitP.Items.Add("02: UNUSED");
                    cboSwRef.Items.Clear();
                    cboSwRef.Items.Add("02: UNUSED");
                    cboSwStop.Items.Clear();
                    cboSwStop.Items.Add("02: UNUSED");
                    break;
            }
           
            //++ 지정한 축의 서보온 상태를 반환합니다.
            CAXM.AxmSignalIsServoOn(m_lAxisNo, ref duOnOff);
            chkServoOn.Checked = Convert.ToBoolean(duOnOff);
        //    DataHelper.dataList[0].ServoOn = Convert.ToBoolean(duOnOff);
       //     DataHelper.dataList[0].Name = "test";
       //    DataHelper.dataList[1].ServoOn = true;
       //     DataHelper.dataList[2].ServoOn = true;
       //     DataHelper.dataList[3].ServoOn = true;
        //    DataHelper.dataList[4].ServoOn = true;



            //++ 선택한 축의 원점신호 Active Level 설정값을 확인합니다.
            duRetCode = CAXM.AxmHomeGetSignalLevel(m_lAxisNo, ref duLevel);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
            ConvertAxmToCombo(ref cboHomeLevel, ref duLevel);

            //++ 선택한 축의 원점검출 속도 설정값을 확인합니다.
            duRetCode = CAXM.AxmHomeGetVel(m_lAxisNo, ref dVelFirst, ref dVelSecond, ref dVelThird, ref dVelLast, ref dAccFirst, ref dAccSecond);
            edtVelFirst.Value = Convert.ToDecimal(dVelFirst);
            edtVelSecend.Value = Convert.ToDecimal(dVelSecond);
            edtVelThird.Value = Convert.ToDecimal(dVelThird);
            edtVelLast.Value = Convert.ToDecimal(dVelLast);
            edtAccFirst.Value = Convert.ToDecimal(dAccFirst);
            edtAccSecond.Value = Convert.ToDecimal(dAccSecond);

            //++ 선택한 축의 원점검출 방법 설정값을 확인합니다.
            duRetCode = CAXM.AxmHomeGetMethod(m_lAxisNo, ref iHomeDir, ref duHomeSignal, ref duZPhaseUse, ref dHomeClrTime, ref dHomeOffset);
            lHomeDir = (uint)iHomeDir;
            ConvertAxmToCombo(ref cboHomeDir, ref lHomeDir);
            ConvertAxmToCombo(ref cboZphaseUse, ref duZPhaseUse);
            ConvertAxmToCombo(ref cboHomeSignal, ref duHomeSignal);

            edtHomeClrTime.Value = Convert.ToDecimal(dHomeClrTime);
            edtHomeOffset.Value = Convert.ToDecimal(dHomeOffset);

            /*
            //++ 선택한 축의 Z상 신호 Active Level 설정값을 확인합니다.
            duRetCode = CAXM.AxmSignalGetZphaseLevel(m_lAxisNo, ref duLevel1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
            ConvertAxmToCombo(ref cboZPhase, ref duLevel1);

            //++ 선택한 축의 Servo Alarm 신호 Active Level/사용유무를 확인합니다.
            duRetCode = CAXM.AxmSignalGetServoAlarm(m_lAxisNo, ref duLevel1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
            ConvertAxmToCombo(ref cboAlarm, ref duLevel1);

            //++ 선택한 축의 Inposition(위치결정완료) 신호 Active Level/사용유무를 확인합니다.
            duRetCode = CAXM.AxmSignalGetInpos(m_lAxisNo, ref duLevel1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
            ConvertAxmToCombo(ref cboInp, ref duLevel1);

            if ((m_uModuleID == (uint)AXT_MODULE.AXT_SMC_R1V04MLIICL) && (m_uModuleID == (uint)AXT_MODULE.AXT_SMC_R1V04MLIICR))
            {
                //++ 선택한 축의 Emergency 신호 Active Level/사용유무를 확인합니다.
                duRetCode = CAXM.AxmSignalGetStop(m_lAxisNo, ref duStopMode, ref duLevel1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
                ConvertAxmToCombo(ref cboEmg, ref duLevel1);
            }

            //++ 선택한 축의 (+/-) End Limit 신호 Active Level/사용유무를 확인합니다.
            duRetCode = CAXM.AxmSignalGetLimit(m_lAxisNo, ref duStopMode, ref duLevel1, ref duLevel2);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = duLevel2 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
            ConvertAxmToCombo(ref cboElimitN, ref duLevel1);
            ConvertAxmToCombo(ref cboElimitP, ref duLevel2);

            //++ 선택된 축의 Servo On/Off 신호 Active Level을 확인합니다.
          //  duRetCode = CAXM.AxmSignalGetServoOnLevel(m_lAxisNo, ref duLevel1);
          //  if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
         //   ConvertAxmToCombo(ref cboServoOn, ref duLevel1);

            //++ 선택된 축의 Alarm Reset 신호 Active Level을 확인합니다.
            duRetCode = CAXM.AxmSignalGetServoAlarmResetLevel(m_lAxisNo, ref duLevel1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;
            ConvertAxmToCombo(ref cboAlarmReset, ref duLevel1);

            //++ 지정한 축에 Software Limit기능을 확인합니다.
            duRetCode = CAXM.AxmSignalGetSoftLimit(m_lAxisNo, ref duSWEnable, ref duSWStopMode, ref duSWRef, ref dSWLimitP, ref dSWLimitN);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) duLevel1 = (uint)AXT_MOTION_LEVEL_MODE.UNUSED;

          //  chkSwEnable.Checked = Convert.ToBoolean(duSWEnable);
            ConvertAxmToCombo(ref cboSwRef, ref duSWRef);
            ConvertAxmToCombo(ref cboSwStop, ref duSWStopMode);
            edtSwPosP.Value = Convert.ToDecimal(dSWLimitP);
            edtSwPosN.Value = Convert.ToDecimal(dSWLimitN);
         */
        }

        private void tmHomeInfo_Tick(object sender, EventArgs e)
        {
            uint duRetCode, duState = 0;
            uint duStepMain = 0, duStepSub = 0;

            //++ 지정한 축의 원점신호의 상태를 확인합니다.
            duRetCode = CAXM.AxmHomeReadSignal(m_lAxisNo, ref duState);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) chkHomeState.Checked = Convert.ToBoolean(duState);

            //++ 지정한 축의 원점신호의 상태를 확인합니다.
            CAXM.AxmHomeGetResult(m_lAxisNo, ref duState);
            if (m_duOldResult != duState)
            {
                labelHomeSearch.Text = TranslateHomeResult(duState);
                m_duOldResult = duState;
            }
            //++ 지정한 축의 원점검색 결과를 확인합니다
            duRetCode = CAXM.AxmHomeGetRate(m_lAxisNo, ref duStepMain, ref duStepSub);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                labelHomeStepMain32.Text = Convert.ToString(duStepMain);
                labelHomeStepSub33.Text = Convert.ToString(duStepSub);
            }
            //++ 지정한 축의 원점검색 진행율을 확인합니다.
            duRetCode = CAXM.AxmHomeGetRate(m_lAxisNo, ref duStepMain, ref duStepSub);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                prgHomeRate.Value = (int)duStepSub;
            }
        }

        private void tmDisplay_Tick(object sender, EventArgs e)
        {
            double dCmdPosition = 0.0;
            double dActPosition = 0.0;
            double dCmdVelocity = 0.0;

            //++ 지정한 축의 지령(Command)위치를 반환합니다.
            CAXM.AxmStatusGetCmdPos(m_lAxisNo, ref dCmdPosition);
            //++ 지정한 축의 실제(Feedback)위치를 반환합니다.
            CAXM.AxmStatusGetActPos(m_lAxisNo, ref dActPosition);
            //++ 지정한 축의 구동 속도를 반환합니다.
            CAXM.AxmStatusReadVel(m_lAxisNo, ref dCmdVelocity);

            textCmdPos.Text = String.Format("{0:0.000}", dCmdPosition);
            textActPos.Text = String.Format("{0:0.000}", dActPosition);
            textCmdVel.Text = String.Format("{0:0.000}", dCmdVelocity);
        }

        public String TranslateHomeResult(uint duHomeResult)
        {
            switch (duHomeResult)
            {
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS: m_strResult = ("[01H] HOME_SUCCESS"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SEARCHING: m_strResult = ("([02H] HOME_SEARCHING"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_GNT_RANGE: m_strResult = ("[10H] HOME_ERR_GNT_RANGE"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_USER_BREAK: m_strResult = ("[11H] HOME_ERR_USER_BREAK"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_VELOCITY: m_strResult = ("[12H] HOME_ERR_VELOCITY"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_AMP_FAULT: m_strResult = ("[13H] HOME_ERR_AMP_FAULT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NEG_LIMIT: m_strResult = ("[14H] HOME_ERR_NEG_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_POS_LIMIT: m_strResult = ("[15H] HOME_ERR_POS_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NOT_DETECT: m_strResult = ("[16H] HOME_ERR_NOT_DETECT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN: m_strResult = ("[FFH] HOME_ERR_UNKNOWN"); break;
            }
            return m_strResult;
        }

        private void btnJogMoveP_MouseUp(object sender, MouseEventArgs e)
        {

            //++ 지정한 축의 Jog구동(모션구동)을 종료합니다.  
            CAXM.AxmMoveSStop(m_lAxisNo);
        }

        private void btnJogMoveP_MouseDown(object sender, MouseEventArgs e)
        {
            uint duRetCode = 0;

            double dVelocity = Math.Abs(Convert.ToDouble(edtJogVel.Value));
            double dAccel = Math.Abs(Convert.ToDouble(edtJogAcc.Value));
            double dDecel = Math.Abs(Convert.ToDouble(edtJogDec.Value));

            //++ 지정한 축을 (+)방향으로 지정한 속도/가속도/감속도로 모션구동합니다.
            duRetCode = CAXM.AxmMoveVel(m_lAxisNo, dVelocity, dAccel, dDecel);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show(String.Format("AxmMoveVel return error[Code:{0:d}]", duRetCode));
        }

        private void btnJogMoveN_MouseUp(object sender, MouseEventArgs e)
        {
            //++ 지정한 축의 Jog구동(모션구동)을 종료합니다.
            CAXM.AxmMoveSStop(m_lAxisNo);
        }

        private void btnJogMoveN_MouseDown(object sender, MouseEventArgs e)
        {
            uint duRetCode = 0;

            double dVelocity = Math.Abs(Convert.ToDouble(edtJogVel.Value));
            double dAccel = Math.Abs(Convert.ToDouble(edtJogAcc.Value));
            double dDecel = Math.Abs(Convert.ToDouble(edtJogDec.Value));

            //++ 지정한 축을 (+)방향으로 지정한 속도/가속도/감속도로 모션구동합니다.
            duRetCode = CAXM.AxmMoveVel(m_lAxisNo, -dVelocity, dAccel, dDecel);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show(String.Format("AxmMoveVel return error[Code:{0:d}]", duRetCode));
        }

        private void btnSetMethod_Click(object sender, EventArgs e)
        {
            uint duRetCode = 0;
            int iHomeDir = 0;
            uint duHomeSignal = 2, duZPhaseUse = 0;
            double dHomeClrTime = 0.0, dHomeOffset = 0.0;

         

            iHomeDir =(int) clsGlVariable.Default.ConvertComboToAxm(ref cboHomeDir);
            duHomeSignal = clsGlVariable.Default.ConvertComboToAxm( ref cboHomeSignal);
            duZPhaseUse = clsGlVariable.Default.ConvertComboToAxm(ref cboZphaseUse);     

            edtHomeOffset.Value = Convert.ToDecimal(dHomeOffset);
            edtHomeClrTime.Value = Convert.ToDecimal(dHomeClrTime);

            //++ 지정한 축의 원점검색 방법을 변경합니다.
            duRetCode = CAXM.AxmHomeSetMethod(m_lAxisNo, iHomeDir, duHomeSignal, duZPhaseUse, dHomeClrTime, dHomeOffset);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show(String.Format("AxmHomeSetMethod return error[Code:{0:d}]", duRetCode));
        }
        // ++ =======================================================================
        // >> ConvertComboToAxm(...) : ComboBox콘트롤에 현재 선택된 Item Index를 모션
        //    보드에 설정할 값으로 변경하는 함수.
        //  - ComboBox에 선택된 Item Index와 실제 모션보드에 설정되는 값이 다르므로 
        //    선택된 문자열에서 모션보드에 설정할 값을 추출해서 반환하는 함수입니다.
        //  - 변환 할 ComboBox 콘트롤의 포인트를 인자로 전달합니다.
        //  - 이 함수를 사용하고자 할 때 데이타 등록을 지정된 양식으로 설정해야됩니다.
        //    Format) XX: String, XX=라이브러리로 전달될 값, String=인자 설명
        //    Ex) "01: HIGH", "00: COMMAND", "04: HomeSensor"
        // --------------------------------------------------------------------------
       /* public uint ConvertComboToAxm(ref ComboBox pCboItem)
        {
            if (pCboItem == null) return 0;

            String strText;

            int iCount, iSeldate;

            iCount = pCboItem.Items.Count;
            if (iCount == 0) return 0;
            iSeldate = pCboItem.SelectedIndex;
            if (iSeldate < 0) return 0;

            strText = pCboItem.Text.Substring(0, 2);
            return Convert.ToUInt32(strText);
        }*/

        private void btnPosClear_Click(object sender, EventArgs e)
        {
            //CAXM.AxmStatusSetCmdPos(m_lAxisNo, 0.0);
            //CAXM.AxmStatusSetActPos(m_lAxisNo, 0.0);	
            //++ Command위치와 Actual위치를 지정한 값으로 설정합니다
            CAXM.AxmStatusSetPosMatch(m_lAxisNo, 0.0);
        }

        private void btnSetVelocity_Click(object sender, EventArgs e)
        {
            uint duRetCode = 0;
            double dVelFirst, dVelSecond, dVelThird, dVelLast, dAccFirst, dAccSecond;

            // 각각의 Edit 콘트롤에서 설정값을 가져옴
            dVelFirst = Math.Abs(Convert.ToDouble(edtVelFirst.Value));
            dVelSecond = Math.Abs(Convert.ToDouble(edtVelSecend.Value));
            dVelThird = Math.Abs(Convert.ToDouble(edtVelThird.Value));
            dVelLast = Math.Abs(Convert.ToDouble(edtVelLast.Value));
            dAccFirst = Math.Abs(Convert.ToDouble(edtAccFirst.Value));
            dAccSecond = Math.Abs(Convert.ToDouble(edtAccSecond.Value));

            //++ 원점검색에 사용되는 단계별 속도를 설정합니다.
            duRetCode = CAXM.AxmHomeSetVel(m_lAxisNo, dVelFirst, dVelSecond, dVelThird, dVelLast, dAccFirst, dAccSecond);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show(String.Format("AxmHomeSetVel return error[Code:{0:d}]", duRetCode));
        }

        private void btnHomeStart_Click(object sender, EventArgs e)
        {
            uint duRetCode = 0;
            //++ 지정한 축에 원점검색을 진행합니다.
            duRetCode = CAXM.AxmHomeSetStart(m_lAxisNo);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show(String.Format("AxmHomeSetStart return error[Code:{0:d}]", duRetCode));
        }

        private void btnMoveStop_Click(object sender, EventArgs e)
        {
            //++ 지정한 축의 구동을 정지합니다.
            CAXM.AxmMoveSStop(m_lAxisNo);
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "AXM files (*.mot)|*.mot|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            char[] nFilename = new char[255];
            string temp = null;

            //++ 함수 실행 성공시 지정한 Mot파일의 설정값으로 모션축 설정이 일괄 변경됩니다.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                temp = openFileDialog1.FileName;
                if (CAXM.AxmMotLoadParaAll(temp) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) MessageBox.Show("File load failed");
            }
            else
            {
                UpdateState();      // 변경된 모션축의 상태와 Control들의 상태를 일치시킵니다.
            }
        }

        private void chkOut00_CheckedChanged(object sender, EventArgs e)
        {
            uint duRetCode;
            int nBitNo;

            nBitNo = Convert.ToInt16(((CheckBox)sender).Tag);
            //++ 선택한 축에 지정한 Bit의 출력접점을 변경합니다.
            duRetCode = CAXM.AxmSignalWriteOutputBit(m_lAxisNo, nBitNo, Convert.ToUInt32(((CheckBox)sender).Checked));
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ((CheckBox)sender).Checked = false;
                MessageBox.Show(String.Format("AxmSignalWriteOutputBit return error[Code:{0:d}]", duRetCode));
            }
        }

        private void chkOut01_CheckedChanged(object sender, EventArgs e)
        {
            uint duRetCode;
            int nBitNo;

            nBitNo = Convert.ToInt16(((CheckBox)sender).Tag);
            //++ 선택한 축에 지정한 Bit의 출력접점을 변경합니다.
            duRetCode = CAXM.AxmSignalWriteOutputBit(m_lAxisNo, nBitNo, Convert.ToUInt32(((CheckBox)sender).Checked));
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ((CheckBox)sender).Checked = false;
                MessageBox.Show(String.Format("AxmSignalWriteOutputBit return error[Code:{0:d}]", duRetCode));
            }
        }

        private void chkOut02_CheckedChanged(object sender, EventArgs e)
        {
            uint duRetCode;
            int nBitNo;

            nBitNo = Convert.ToInt16(((CheckBox)sender).Tag);
            //++ 선택한 축에 지정한 Bit의 출력접점을 변경합니다.
            duRetCode = CAXM.AxmSignalWriteOutputBit(m_lAxisNo, nBitNo, Convert.ToUInt32(((CheckBox)sender).Checked));
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ((CheckBox)sender).Checked = false;
                MessageBox.Show(String.Format("AxmSignalWriteOutputBit return error[Code:{0:d}]", duRetCode));
            }
        }

        private void chkOut03_CheckedChanged(object sender, EventArgs e)
        {
            uint duRetCode;
            int nBitNo;

            nBitNo = Convert.ToInt16(((CheckBox)sender).Tag);
            //++ 선택한 축에 지정한 Bit의 출력접점을 변경합니다.
            duRetCode = CAXM.AxmSignalWriteOutputBit(m_lAxisNo, nBitNo, Convert.ToUInt32(((CheckBox)sender).Checked));
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ((CheckBox)sender).Checked = false;
                MessageBox.Show(String.Format("AxmSignalWriteOutputBit return error[Code:{0:d}]", duRetCode));
            }
        }

        private void chkOut04_CheckedChanged(object sender, EventArgs e)
        {
            uint duRetCode;
            int nBitNo;

            nBitNo = Convert.ToInt16(((CheckBox)sender).Tag);
            //++ 선택한 축에 지정한 Bit의 출력접점을 변경합니다.
            duRetCode = CAXM.AxmSignalWriteOutputBit(m_lAxisNo, nBitNo, Convert.ToUInt32(((CheckBox)sender).Checked));
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ((CheckBox)sender).Checked = false;
                MessageBox.Show(String.Format("AxmSignalWriteOutputBit return error[Code:{0:d}]", duRetCode));
            }
        }

        private void chkSwEnable_CheckedChanged(object sender, EventArgs e)
        {
            uint duRetCode;
            double dSWLimitP, dSWLimitN;
            uint duSWEnable, duSWStopMode, duSWRef;

            duSWEnable = Convert.ToUInt32(chkSwEnable.Checked);
            duSWStopMode = clsGlVariable.Default.ConvertComboToAxm(ref cboSwStop);
            duSWRef = clsGlVariable.Default.ConvertComboToAxm(ref cboSwRef);
            dSWLimitP = Convert.ToDouble(edtSwPosP.Value);
            dSWLimitN = Convert.ToDouble(edtSwPosN.Value);

            //++ 지정한 축에 Software Limit기능을 설정합니다.
            // ※ [INFO] Software Limit 기능은 원점검색시 무시됩니다. 원점검색이 완료되면 자동으로 이전 상태가 됩니다.
            duRetCode = CAXM.AxmSignalSetSoftLimit(m_lAxisNo, duSWEnable, duSWStopMode, duSWRef, dSWLimitP, dSWLimitN);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                MessageBox.Show(String.Format("AxmSignalSetSoftLimit return error[Code:{0:d}]", duRetCode));
                chkSwEnable.Checked = false;
            }
        }

        private void chkServoOn_CheckedChanged(object sender, EventArgs e)
        {
            uint duOnOff;

            //++ 지정 축의 Servo On/Off 신호를 출력합니다.
            duOnOff = (uint)Convert.ToInt32(chkServoOn.Checked);
            CAXM.AxmSignalServoOn(m_lAxisNo, duOnOff);
          
        }


        // ++ =======================================================================
        // >> ConvertAxmToCombo(...) : 지정한 모션보드에 설정값과 ComboBox콘트롤의  
        //    Item과 일치하는 Item을 찾아 선택하는 함수.
        //  - 실제 모션보드에 설정되는 값과 ComboBox의 Item Index와  다르므로 지정한   
        //    값과 일치하는 Item을 찾아 선택하고 선택된 Item의 Index를 반환합니다.
        //  - 변경하고자하는 ComboBox 콘트롤의 포인트를 인자로 전달합니다.
        //  - 이 함수를 사용하고자 할 때 데이타 등록을 지정된 양식으로 설정해야됩니다.
        //    Format) XX: String, XX=라이브러리로 전달될 값, String=인자 설명
        //    Ex) "01: HIGH", "00: COMMAND", "04: HomeSensor"
        // --------------------------------------------------------------------------
        public uint ConvertAxmToCombo(ref ComboBox pCboItem, ref uint diCurData)
        {
            if (pCboItem == null) return 0;

            uint lCount, lSeldata;
            String strText;

            lCount = (uint)pCboItem.Items.Count;

            if (lCount == 0) return 0;

            for (int i = 0; i < lCount; i++)
            {
                strText = pCboItem.Items[i].ToString();
                lSeldata = Convert.ToUInt32(strText.Substring(0, 2));
                if (lSeldata == (uint)diCurData)
                {
                    pCboItem.SelectedIndex = i;
                    return (uint)i;
                }
            }
            pCboItem.SelectedIndex = 0;
            return 0;
        }

        private void btnMove1_Click(object sender, EventArgs e)
        {
            double dMovePos, dMoveVel, dMoveAcc, dMoveDec;

            dMovePos = Convert.ToDouble(edtMovePos.Text);
            dMoveVel = Convert.ToDouble(edtMoveVel.Text);
            dMoveAcc = Convert.ToDouble(edtMoveAcc.Text);
            dMoveDec = Convert.ToDouble(edtMoveDec.Text);

            //++ 지정한 축을 지정한 거리(또는 위치)/속도/가속도/감속도로 모션구동하고 모션 종료여부와 상관없이 함수를 빠져나옵니다.
            CAXM.AxmMoveStartPos(m_lAxisNo, dMovePos, dMoveVel, dMoveAcc, dMoveDec);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

            //++ 지정한 축의 구동을 감속정지합니다.
            CAXM.AxmMoveSStop(m_lAxisNo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isThread = false;
            m_Thread.Abort();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartThread();
        }

        private void cboHomeLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint duLevel;
            uint duRetCode = 0;
       
            duLevel = clsGlVariable.Default.ConvertComboToAxm(ref cboHomeLevel);
            //++ 지정한 축의 원점신호 Active Level을 변경합니다.
            duRetCode = CAXM.AxmHomeSetSignalLevel(m_lAxisNo, duLevel);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                MessageBox.Show(String.Format("AxmHomeSetSignalLevel return error[Code:{0:d}]", duRetCode));
        }

        private void setGridStyles()
        {


            //그리드에 체크박스 넣고 체크박스로 멀티 row 선택하기
          //  gridView1.OptionsSelection.MultiSelect = true;
          //  gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
          //  gridView1.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
          //  gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;

            gridControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
                gridControl1.LookAndFeel.UseDefaultLookAndFeel = false; // <<<<<<<<  
                gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;

                gridView1.Appearance.HeaderPanel.BackColor = Color.White;
                gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
                gridView1.OptionsView.ColumnAutoWidth = true;
            //  gridView1.Appearance.HeaderPanel.ForeColor = Color.FromArgb(2, 178, 227);
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
             gridView1.Appearance.Empty.BackColor = Color.Black;  // 데이터 가 없는 빈 공간 

           

            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                    gridView1.Columns[i].AppearanceHeader.Options.UseBackColor = true;

                  //  gridView1.Columns[i].AppearanceHeader.BackColor = colorDark;  // 헤더 색 

                    gridView1.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    gridView1.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gridView1.Columns[i].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                   gridView1.Columns[i].OptionsColumn.ReadOnly = true;
                //  gridView1.Columns[i].Image = EzMotion.Properties.Resources.;
                //   gridView1.Columns[i].OptionsColumn.AllowEdit = true;

            }

                gridView1.RowHeight = 40;
                gridView1.ColumnPanelRowHeight = 50;
              gridView1.OptionsBehavior.Editable = false;
         
   
            //   gridView1.Appearance.Row.BackColor = Color.AliceBlue;
            //  gridView1.Appearance.Row.BackColor2 = Color.FromArgb(18, 49, 77);
            //  gridView1.Appearance.Row.ForeColor = Color.White;
            // gridView1.OptionsView.ShowFooter = true;

            //  gridView1.OptionsView.AllowCellMerge = true;
            //  gridView1.OptionsView.ColumnAutoWidth = true;
            //  gridView1.OptionsView.RowAutoHeight = true;
            //   gridView1.OptionsBehavior.Editable = true;
            //    gridView1.OptionsView.ShowAutoFilterRow = true;
            gridView1.BestFitColumns();
                // GridView 짝수라인의 색상을 변경한다.
             
          

        }

     

   
     

    }
}
