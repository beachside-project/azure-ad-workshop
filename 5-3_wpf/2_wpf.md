# WPF での認証

ここでは、マイクロソフトで公開している WPF のサンプルコードを使って Azure AD のマルチテナントでの認証を実装します。


## 📜 サンプルコードのダウンロード

ここでは、以下で公開されているサンプルコードを利用します。

- https://github.com/Azure-Samples/active-directory-dotnet-desktop-msgraph-v2

リポジトリーを `git clone` でクローンするかダウンロードして zip を展開して Visual Studio でコードを開きます。

<br>

## 📜 認証を構成する

### NugGet パッケージ

Visual Studio の上部にある検索に「nuget」と入力して **ソリューションの NuGet パッケージの管理** をクリックします。

![image](./images/02_01.png)

<br>

**インストール済み** をみると、**Microsoft.Identity.Client** のパッケージがインストールされています。これが .NET 用の MSAL のライブラリーになります。"MSAL" というパッケージ名ではない点はご注意ください。

![image](./images/02_02.png)

### App.xaml.cs の確認とマルチテナントの構成

ソリューションエクスプローラーで App.xaml.cs を開きます。認証を行うためのオブジェクトは、`IPublicClientApplication` で定義されている `_clientApp` になります。static コンストラクターでインスタンスを初期化をする際に Azure Active Directory の情報を読み込んでいます。

![image](./images/02_03.png)

`ClientId` に自身の Azure Active Directory のクライアント ID をセットします。

`Tenant` には `common` が設定されています。Azure Active Directory のアプリでマルチテナントを構成している場合、設定する値のルールは以下となっています。

|設定する値|概要|
|---|---|
|`organizations`|任意の組織のアカウントでログインできます。|
|`common`|任意の組織のアカウントと Microsoft アカウントでログインできます。|
|`consumers`|Microsoft アカウントのみでログインできます。|
|テナント ID|指定のテナントのアカウントのみログインできます（シングルテナントの状態）|

シングルテナント・マルチテナントを構成する際の注意点は以下です。

- Azure Active Directory のアプリで **サポートされているアカウントの種類** をマルチテナントを構成していても、クライアントの設定次第で構成が変わります。
- Azure Active Directory のアプリで **サポートされているアカウントの種類** で設定していない種類の認証はできません。例として、Azure Active Directory のアプリでシングルテナント構成をしていると、クライアントアプリでマルチテナントの構成をして、別組織のアカウントでログインしようとしてもログインできません。

> 🔎 このワークショップでいくつかのアプリ登録をしてきましたが、実際の開発ではどんなユースケースのアプリケーションを開発したいかに応じて **サポートされているアカウントの種類** を構成します。


### 