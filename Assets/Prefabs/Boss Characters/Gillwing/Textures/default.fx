////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  ���甧.fx  �h�D�h�D
//
//  ��full.fx v1.2(���͉��P) ��VSM(���ڂ뎁 SpotLight2) ���x�[�X�Ƀf�[�^P���쐬���ꂽ
//  ��AdultShaderS2.fx v0.14(�f�[�^P) ��SeriousShader.fx v0.21(�f�[�^P) ���x�[�X�ɁA
//  ��full.fx v2.0(���͉��P) ��ExcellentShadow(���ڂ뎁) ���ٕ����t�B���^�����O(furia��)
//  �����ʔ���(�r�[���}��P MechanicMirror) ��Full���[�h(�r�[���}��P Mechanic)
//  �������Օ��}�b�s���O(DirectX�T���v��)
//    �Ȃǂ̋@�\��t�����������ł��B
//
////////////////////////////////////////////////////////////////////////////////////////////////////
// �p�����[�^�錾

// DarkShader�R���̃p�����[�^ ----------------------------------------------------------------------
//#define EDGE_ENABLED // �G�b�W��`�悵�Ȃ� (�`�悷��ꍇ�A����//���O��)
//#define SHADOW_ENABLED // �e�i��Z���t�V���h�E�j��`�悵�Ȃ� (�`�悷��ꍇ�A����//���O��)
#define CULLING_ENABLED // �������ŃZ���t�V���h�E���ςȎ��ɁA����//��t���ċ����I�ɗ��ʕ`��(�d��)
//#define USE_HDR // �I�[�g���~�i�X�ɔ���������(�����Ȃ��ꍇ�A����//)
//#define USE_DARK // �F�̈��^���Â���o�͂ł���悤�Ɋg��(���Ȃ��ꍇ�A����//)

// AdultShader / SeriousShader �R���̐ݒ� ----------------------------------------------------------

#define ShadowDarkness   1      // �Z���t�V���h�E�̍ő�Â�
#define UnderSkinDiffuse 0.5    // �牺�U��
#define EyeLightPower    2.0    // ���������ł̐F�����̕ω�
#define ToonPower        0.5    // �e�̈Â�
#define OverBright       1.7    // ����ъo��̖��邳�␳
#define FresnelCoef      4.0    // �t���l�����̌W��
#define FresnelFact      50     // �t���l����

// �\�t�g�V���h�E �␳�W��
#define SOFTSHADOW_DISTANCE  0.1    // �\�t�g�V���h�E��ł��؂鋗��(�������قǉ���)
#define SOFTSHADOW_THRESHOLD 0.0005 // �\�t�g�V���h�E�␳�l �傫���قǉe������

// �V���h�E�}�b�v�T�C�Y(MMD�N����:2048, Ctrl+G�g�p:4096)
#define SHADOWMAP_WIDTH  2048
#define SHADOWMAP_HEIGHT 2048

// �e�̔Z��(ExcellentShadow�R���̃p�����[�^) -------------------------------------------------------
float X_SHADOWPOWER   = 1.0; //�A�N�Z�T���e�Z��
float PMD_SHADOWPOWER = 1.0; //���f���e�Z��

// �}�b�s���O�g�p�ݒ� ------------------------------------------------------------------------------

// ����2�e�N�X�`��(�e�N�X�`���̐F��ς���A�y��)
//#define USE_TEX_BLEND // �g��? (�g��Ȃ��ꍇ�A����//)
    #define TEX_BLEND_PATH "TexBlend.png"
    float TexBlendRatio = 1.0; // �F�u�����h�䗦(0:���摜100% �` 1:�u���摜100%)
    float TexBlendRepeat = 1.0; // �u�����h�e�N�X�`���̌J��Ԃ��� ���폜�s��

// ����2�X�t�B�A(���͂̌��̗l�q��ς��ĉA�e�t���A�y��)
//#define IGNORE_SPHERE_1ST // �����̃X�t�B�A�𖳎�����? (���p����ꍇ�A����//)

//#define USE_SPHERE_2ND // �g��? (�g��Ȃ��ꍇ�A����//)
    #define SPHERE_2ND_PATH "0.rimlight2.png"
    #define SPHERE_ADD // �g�p����X�t�B�A�͉��Z�^�C�v? (��Z�^�C�v�̏ꍇ�A����//)    
    float3 AmbientBoost = float3(1.0,1.0,1.0) * 0.9; // �X�t�B�A�ȊO�̖��邳�Ɋ|����{��
    float3 SphereBoost  = float3(1.0,1.0,1.0) * 1.0; // �X�t�B�A�̖��邳�Ɋ|����{��

float NormalMapRepeat = 1.0; // (�@���E�����E�X�y�L����)�}�b�v�̌J��Ԃ��� ���폜�s��

// ���X�y�L�����}�b�v(���˗���ς��ĉA�e�t���A�y��)
//#define USE_SPECULAR_MAP // �g��? (�g��Ȃ��ꍇ�A����//)
    #define SPECULAR_MAP_PATH "tot_body_spc.png"

// ���@���}�b�v(�@���̕�����ς��ĉ��ʊ��A�d��)
#define USE_NORMAL_MAP // �g��? (�g��Ȃ��ꍇ�A����//)
    #define NORMAL_MAP_PATH "default_nrm.png"
  #define INVERSE_X // �@���}�b�v�� X�v�f�̕������t�]
  #define INVERSE_Y // �@���}�b�v�� Y�v�f�̕������t�]
    #define NormalMapSize float2(256, 256) // �m�[�}���}�b�v�̃T�C�Y(�����Օ��}�b�v���g���ꍇ�A�v�w��)

// �������}�b�v(�ʒu�����炵�ĉ��ʊ��A�d�� ���@���}�b�v��L���ɂ���K�v����)
//#define USE_HEIGHT_MAP // �g��? (�g��Ȃ��ꍇ�A����//)
    #define HEIGHT_MAP_PATH "HeightMap.png"
  //#define INVERSE_HIGHT // �����}�b�v�̍������t�] (�ʏ�:0��`1��, �t�]:1��`0��)
    #define HEIGHT_MAP_METHOD 2 // 1:�����}�b�v(���d), 2:�����Օ��}�b�v(���d, ���̊���)
    // �����}�b�s���O(PM)�g�p���̐ݒ�
    static float HeightScalePM = 0.03 * sqrt(NormalMapRepeat); // �[��(�����₵������Ɣj�]���܂�)
    // �����Օ��}�b�s���O(POM)�g�p���̐ݒ�
    float       HeightScalePOM = 0.1; // �[��
    #define  POM_SMOOTHING_MIN   10   // ���炩�ɂ��邽�߂̃T���v�����F�ŏ��l(�����₷�ƒ��d���Ȃ�܂�)
    #define  POM_SMOOTHING_MAX   20   //             �V              �F�ő�l(          �V          )
    #define  DISPLAY_SHADOWS          // �\�t�g�V���h�E���g����
    float    SoftShadow        = 0.5; // �\�t�g�V���h�E�̂ڂ�����
    #define  USE_POM_LOD              // LOD(�߂���POM�ŏڍׂɁA�����͖@���}�b�v�őe���`��)���g����
    int      MipLevelSikiiLOD  = 3;   // LOD臒l(MIP���x�������̒l�ȏ�Ȃ�A�ʏ�̖@���}�b�v�ŕ\��)

// �����ʔ���
//#define USE_MIRROR // �g��?
    float MirrorParam = 0.5; // ���˗�(0�`1)
    float ReflectSpecularSikii = 0.1; // �ގ��̃X�y�L�����F���X�y�L�����}�b�v�̐ԗv�f�����̒l�ȏ�Ȃ�
                                      // ���ʔ��˂�����(0.0�`1.0) ��ɔ��˂���������� 0 ���w��

// ���t�����[�h
//#define USE_FULLMODE // �g��? �t�����[�h�̏ڍׂ͐�������
    #define DefSubset "8-13,15-19" // �t�����[�h�̑ΏۂɁu���Ȃ��v�T�u�Z�b�g
                                   // �@���E�����E�X�y�L�����}�b�v�𖳎����܂��B

// �ٕ����t�B���^�����O�^�~�b�v�}�b�v�ݒ� ----------------------------------------------------------

// ���ٕ����t�B���^�����O���g�p���邩
//  0 : �g�p���Ȃ� (��i��, �y��, MMD7.39.�ȑO����)
//  1 : �Ǝ��Ƀ~�b�v�}�b�v���쐬���Ďg�p���� (���`���i��, �d��(�璷�ȏ������K�v), MMD7.39.�ȑO����)
//  2 : MMD�{�̂̃~�b�v�}�b�v�𗘗p���Ďg�p���� (���i��, ����, MMD64bit�Ł^32bit�}���`�R�A�Ō���)
#define ANISOTROPY_TYPE 2

// �ٕ����t�B���^�����O�̃T���v�����O��(1�`16��I���\�A�����قǏd�������i��)
#define MAX_ANISOTROPY 16

// �Ǝ��Ƀ~�b�v�}�b�v���쐬����ꍇ( ANISOTROPY_TYPE 1 )�́A�o�b�t�@�T�C�Y
//  �E���f���ɐ��l�ȏ�̍��𑜓x�ȃe�N�X�`�����g�p����Ă���ꍇ�A���₵�Ă��������B���d���Ȃ�܂�
//  �E�}���`�R�A�łɃo�[�W�����A�b�v�� ANISOTROPY_TYPE 2 ���g���������y���Ȃ�܂��B
#define TEXBUFFWIDTH  512
#define TEXBUFFHEIGHT 512
#define TEXBUFFSIZE { TEXBUFFWIDTH, TEXBUFFHEIGHT }

// ���ʏ����t�@�C����ǂݍ���
#include "_dSASCommon.fxsub"
