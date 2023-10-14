import Head from "next/head";
import dynamic from "next/dynamic";

const NoSSRMap = dynamic(() => import("@/components/Map"), { ssr: false });

export default function Home() {

  return (
    <>
      <Head>
        <title>Карта отделений</title>
        <link
          rel="icon"
          type="image/png"
          sizes="32x32"
          href="/favicon-32x32.png"
        />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main>
        <NoSSRMap />




      </main>
    </>
  );
}
