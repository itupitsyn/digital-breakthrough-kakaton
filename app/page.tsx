"use server";

import { Recommended } from "@/components/Recommended";
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
      const recommendedResponse = await axios.get<Video[]>(`${process.env.API_URL}/recsys/next`, {
        headers: {
          [TOKEN_API_HEADER]: token.value,
        },
      });
      recVideos = recommendedResponse.data;
      isOk = true;
    } catch {
      ///
    }
  }

  return (
    <main className="mt-10 flex flex-col items-stretch gap-10">
      {recVideos ? (
        <>
          <Recommended videos={recVideos} />
          <hr className="border-cyan-500" />
          {/* <VideoGrid videos={videosResponse.data} /> */}
        </>
      ) : (
        <div className="flex justify-center text-2xl font-bold">Сначала залогиньтесь</div>
      )}
    </main>
  );
}
