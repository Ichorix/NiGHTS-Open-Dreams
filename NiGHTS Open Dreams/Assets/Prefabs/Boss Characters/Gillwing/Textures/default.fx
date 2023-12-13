////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  さら肌.fx  ドゥドゥ
//
//  □full.fx v1.2(舞力介入P) □VSM(そぼろ氏 SpotLight2) をベースにデータPが作成された
//  ■AdultShaderS2.fx v0.14(データP) ■SeriousShader.fx v0.21(データP) をベースに、
//  □full.fx v2.0(舞力介入P) □ExcellentShadow(そぼろ氏) □異方向フィルタリング(furia氏)
//  □鏡面反射(ビームマンP MechanicMirror) □Fullモード(ビームマンP Mechanic)
//  □視差遮蔽マッピング(DirectXサンプル)
//    などの機能を付け加えた物です。
//
////////////////////////////////////////////////////////////////////////////////////////////////////
// パラメータ宣言

// DarkShader由来のパラメータ ----------------------------------------------------------------------
//#define EDGE_ENABLED // エッジを描画しない (描画する場合、左の//を外す)
//#define SHADOW_ENABLED // 影（非セルフシャドウ）を描画しない (描画する場合、左の//を外す)
#define CULLING_ENABLED // 屋内等でセルフシャドウが変な時に、左に//を付けて強制的に両面描画(重い)
//#define USE_HDR // オートルミナスに反応させる(させない場合、左に//)
//#define USE_DARK // 色領域を真っ暗から出力できるように拡張(しない場合、左に//)

// AdultShader / SeriousShader 由来の設定 ----------------------------------------------------------

#define ShadowDarkness   1      // セルフシャドウの最大暗さ
#define UnderSkinDiffuse 0.5    // 皮下散乱
#define EyeLightPower    2.0    // 視線方向での色合いの変化
#define ToonPower        0.5    // 影の暗さ
#define OverBright       1.7    // 白飛び覚悟の明るさ補正
#define FresnelCoef      4.0    // フレネル項の係数
#define FresnelFact      50     // フレネル項

// ソフトシャドウ 補正係数
#define SOFTSHADOW_DISTANCE  0.1    // ソフトシャドウを打ち切る距離(小さいほど遠い)
#define SOFTSHADOW_THRESHOLD 0.0005 // ソフトシャドウ補正値 大きいほど影が薄い

// シャドウマップサイズ(MMD起動時:2048, Ctrl+G使用:4096)
#define SHADOWMAP_WIDTH  2048
#define SHADOWMAP_HEIGHT 2048

// 影の濃さ(ExcellentShadow由来のパラメータ) -------------------------------------------------------
float X_SHADOWPOWER   = 1.0; //アクセサリ影濃さ
float PMD_SHADOWPOWER = 1.0; //モデル影濃さ

// マッピング使用設定 ------------------------------------------------------------------------------

// ●第2テクスチャ(テクスチャの色を変える、軽い)
//#define USE_TEX_BLEND // 使う? (使わない場合、左に//)
    #define TEX_BLEND_PATH "TexBlend.png"
    float TexBlendRatio = 1.0; // 色ブレンド比率(0:元画像100% ～ 1:置換画像100%)
    float TexBlendRepeat = 1.0; // ブレンドテクスチャの繰り返し数 ※削除不可

// ●第2スフィア(周囲の光の様子を変えて陰影付け、軽い)
//#define IGNORE_SPHERE_1ST // 既存のスフィアを無視する? (併用する場合、左に//)

//#define USE_SPHERE_2ND // 使う? (使わない場合、左に//)
    #define SPHERE_2ND_PATH "0.rimlight2.png"
    #define SPHERE_ADD // 使用するスフィアは加算タイプ? (乗算タイプの場合、左に//)    
    float3 AmbientBoost = float3(1.0,1.0,1.0) * 0.9; // スフィア以外の明るさに掛ける倍率
    float3 SphereBoost  = float3(1.0,1.0,1.0) * 1.0; // スフィアの明るさに掛ける倍率

float NormalMapRepeat = 1.0; // (法線・高さ・スペキュラ)マップの繰り返し数 ※削除不可

// ●スペキュラマップ(反射率を変えて陰影付け、軽い)
//#define USE_SPECULAR_MAP // 使う? (使わない場合、左に//)
    #define SPECULAR_MAP_PATH "tot_body_spc.png"

// ●法線マップ(法線の方向を変えて凹凸感、重い)
#define USE_NORMAL_MAP // 使う? (使わない場合、左に//)
    #define NORMAL_MAP_PATH "default_nrm.png"
  #define INVERSE_X // 法線マップの X要素の方向を逆転
  #define INVERSE_Y // 法線マップの Y要素の方向を逆転
    #define NormalMapSize float2(256, 256) // ノーマルマップのサイズ(視差遮蔽マップを使う場合、要指定)

// ●視差マップ(位置をずらして凹凸感、重い ※法線マップを有効にする必要あり)
//#define USE_HEIGHT_MAP // 使う? (使わない場合、左に//)
    #define HEIGHT_MAP_PATH "HeightMap.png"
  //#define INVERSE_HIGHT // 高さマップの高さを逆転 (通常:0低～1高, 逆転:1低～0高)
    #define HEIGHT_MAP_METHOD 2 // 1:視差マップ(少重), 2:視差遮蔽マップ(激重, 立体感高)
    // 視差マッピング(PM)使用時の設定
    static float HeightScalePM = 0.03 * sqrt(NormalMapRepeat); // 深さ(※増やしすぎると破綻します)
    // 視差遮蔽マッピング(POM)使用時の設定
    float       HeightScalePOM = 0.1; // 深さ
    #define  POM_SMOOTHING_MIN   10   // 滑らかにするためのサンプル数：最小値(※増やすと超重くなります)
    #define  POM_SMOOTHING_MAX   20   //             〃              ：最大値(          〃          )
    #define  DISPLAY_SHADOWS          // ソフトシャドウを使うか
    float    SoftShadow        = 0.5; // ソフトシャドウのぼかし率
    #define  USE_POM_LOD              // LOD(近くはPOMで詳細に、遠くは法線マップで粗く描画)を使うか
    int      MipLevelSikiiLOD  = 3;   // LOD閾値(MIPレベルがこの値以上なら、通常の法線マップで表示)

// ●鏡面反射
//#define USE_MIRROR // 使う?
    float MirrorParam = 0.5; // 反射率(0～1)
    float ReflectSpecularSikii = 0.1; // 材質のスペキュラ色かスペキュラマップの赤要素がこの値以上なら
                                      // 鏡面反射させる(0.0～1.0) 常に反射させたければ 0 を指定

// ●フルモード
//#define USE_FULLMODE // 使う? フルモードの詳細は説明書で
    #define DefSubset "8-13,15-19" // フルモードの対象に「しない」サブセット
                                   // 法線・視差・スペキュラマップを無視します。

// 異方向フィルタリング／ミップマップ設定 ----------------------------------------------------------

// ●異方向フィルタリングを使用するか
//  0 : 使用しない (低品質, 軽い, MMD7.39.以前向け)
//  1 : 独自にミップマップを作成して使用する (中～高品質, 重い(冗長な処理が必要), MMD7.39.以前向け)
//  2 : MMD本体のミップマップを利用して使用する (高品質, 普通, MMD64bit版／32bitマルチコア版向け)
#define ANISOTROPY_TYPE 2

// 異方向フィルタリングのサンプリング数(1～16を選択可能、多いほど重いが高品質)
#define MAX_ANISOTROPY 16

// 独自にミップマップを作成する場合( ANISOTROPY_TYPE 1 )の、バッファサイズ
//  ・モデルに数値以上の高解像度なテクスチャが使用されている場合、増やしてください。※重くなります
//  ・マルチコア版にバージョンアップし ANISOTROPY_TYPE 2 を使った方が軽くなります。
#define TEXBUFFWIDTH  512
#define TEXBUFFHEIGHT 512
#define TEXBUFFSIZE { TEXBUFFWIDTH, TEXBUFFHEIGHT }

// 共通処理ファイルを読み込む
#include "_dSASCommon.fxsub"
