import { getRandomVideos } from "@/utils/api";
import { NextResponse } from "next/server";

const data = getRandomVideos(10);

export const GET = async () => {
  return new NextResponse(JSON.stringify(data));
};
