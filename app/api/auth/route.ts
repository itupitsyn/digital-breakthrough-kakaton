import { Auth } from "@/model/auth";
import axios from "axios";
import { NextResponse } from "next/server";

export const POST = async () => {
  try {
    const response = await axios.post<Auth>(`${process.env.API_URL}/auth`);
    return new NextResponse(JSON.stringify(response.data));
  } catch (error) {
    return new NextResponse(JSON.stringify(error), { status: 500 });
  }
};
