"use server";

import { RecommendedHeader } from "@/components/RecommendedHeader";
// import { VideoCarousel } from "@/components/VideoCarousel";
import { VideoGrid } from "@/components/VideoGrid";
import { TOKEN_API_HEADER, TOKEN_COOKIES } from "@/constants/token";
import { Video } from "@/model/video";
import axios from "axios";
import { cookies } from "next/headers";

export default async function Home() {
  const token = cookies().get(TOKEN_COOKIES);

  let isOk = false;

  let recVideos: Video[] | undefined;
  if (token) {
    try {
      const recommendedResponse = await axios.post<Video[]>(`${process.env.API_URL}/recsys/next`, undefined, {
        headers: {
          [TOKEN_API_HEADER]: token.value,
        },
      });
      recVideos = recommendedResponse.data;
      isOk = true;
    } catch (e) {
      console.log(e);
    }
  }

  return (
    <main className="mt-12 flex flex-col items-stretch gap-6">
      {recVideos ? (
        <>
          <RecommendedHeader />
          {/* <VideoCarousel videos={recVideos} /> */}
          {/* <hr className="border-cyan-500" /> */}
          <VideoGrid videos={recVideos} />
        </>
      ) : (
        <div className="flex justify-center text-2xl font-bold">Сначала залогиньтесь</div>
      )}
    </main>
  );
}
