import { TOKEN_API_HEADER, TOKEN_COOKIES } from "@/constants/token";
import { Auth } from "@/model/auth";
import axios from "axios";
import { NextRequest, NextResponse } from "next/server";

type RouteParams = {
  params: {
    id: string;
  };
};

export const POST = async (req: NextRequest, { params }: RouteParams) => {
  const token = req.cookies.get(TOKEN_COOKIES);
  try {
    const response = await axios.post<Auth>(`${process.env.API_URL}/recsys/like/${params.id}`, undefined, {
      headers: {
        [TOKEN_API_HEADER]: token?.value,
      },
    });
    return new NextResponse(JSON.stringify(response.data));
  } catch (error) {
    return new NextResponse(JSON.stringify(error), { status: 500 });
  }
};
