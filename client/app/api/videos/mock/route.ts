import { getRandomVideos } from "@/utils/api";
import { NextResponse } from "next/server";

const data = getRandomVideos(100);

export const GET = () => {
  return new NextResponse(JSON.stringify(data));
};
